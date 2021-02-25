using BusinessLogicalLayer.Interfaces;
using Common;
using DataAccessLayer;
using Metadata;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogicalLayer
{
    public class StudentBLL : BaseValidator<Student>, IStudentService
    {

        private readonly IGenerateRegister _generateRegister;
        public StudentBLL(IGenerateRegister generateRegister)
        {
            this._generateRegister = generateRegister;
        }

        public override Response Validate(Student student)
        {
            AddError(student.StudentName.IsValidName());
            AddError(student.Cpf.IsValidCPF());
            AddError(student.PhoneNumber.IsValidPhoneNumber());
            return base.Validate(student);
        }

        public async Task<SingleResponse<int>> Insert(Student student)
        {
            Response response = Validate(student);

            if (response.Success)
            {
                student.Cpf = student.Cpf.RemoveMaskCPF();
                student.PhoneNumber = student.PhoneNumber.RemoveMaskPhoneNumber();
                student.Passcode = student.Cpf;
                student.Passcode = student.Passcode.EncryptPassword();
                student.Active = true;

                do
                {
                    student.Register = _generateRegister.GenerateRandonRegister();
                } while (!IsUniqueRegister(student.Register).Result.Success);

                try
                {
                    using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                    {

                        dataBase.Students.Add(student);
                        await dataBase.SaveChangesAsync();
                        SingleResponse<int> data = new SingleResponse<int>();
                        data.Data = student.ID;
                        data.Success = true;
                        data.Message = "Estudante cadastrado com sucesso.";
                        return data;
                    }
                }
                catch (Exception ex)
                {
                    return ResponseMessage.CreateSingleErrorResponse<int>(ex);
                }
            }
            return ResponseMessage.CreateSingleErrorResponse<int>();
        }

        public async Task<Response> Update(Student student)
        {
            Response validationResponse = Validate(student);
            if (!validationResponse.Success)
            {
                return validationResponse;
            }
            try
            {
                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    dataBase.Entry(student).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    await dataBase.SaveChangesAsync();
                }
                return ResponseMessage.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                return ResponseMessage.CreateErrorResponse(ex);
            }
        }

        public async Task<SingleResponse<Student>> ChangeStatus(int id)
        {
            try
            {
                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    Student student = dataBase.Students.Find(id);
                    if (student.Active == false)
                    {
                        student.Active = true;
                    }
                    else
                    {
                        student.Active = false;
                    }
                    await dataBase.SaveChangesAsync();
                    return ResponseMessage.CreateSingleSuccessResponse<Student>(student);
                }
            }
            catch (Exception ex)
            {
                ResponseMessage.CreateErrorResponse(ex);
                return ResponseMessage.CreateNotFoundData<Student>();
            }
        }

        public async Task<SingleResponse<Student>> GetByID(int id)
        {
            try
            {
                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    Student student = await dataBase.Students.FirstOrDefaultAsync(p => p.ID == id);
                    if (student == null)
                    {
                        return ResponseMessage.CreateNotFoundData<Student>();
                    }
                    return ResponseMessage.CreateSingleSuccessResponse<Student>(student);
                }
            }
            catch (Exception ex)
            {
                SingleResponse<Student> student = (SingleResponse<Student>)ResponseMessage.CreateErrorResponse(ex);
                return student;
            }
        }

        public async Task<QueryResponse<Student>> GetAll()
        {
            try
            {
                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    List<Student> students = await dataBase.Students.ToListAsync();
                    return ResponseMessage.CreateQuerySuccessResponse<Student>(students);
                }
            }
            catch (Exception ex)
            {
                QueryResponse<Student> student = (QueryResponse<Student>)ResponseMessage.CreateErrorResponse(ex);
                return student;
            }
        }

        public async Task<SingleResponse<Student>> IsUniqueRegister(string register)
        {
            try
            {
                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    Student student = await dataBase.Students.FirstOrDefaultAsync(p => p.Register == register);

                    if (student == null)
                    {
                        return ResponseMessage.CreateSingleSuccessResponse<Student>(student);
                    }
                    return ResponseMessage.CreateNotFoundData<Student>();
                }
            }
            catch (Exception ex)
            {
                SingleResponse<Student> student = (SingleResponse<Student>)ResponseMessage.CreateErrorResponse(ex);
                return student;
            }
        }

        public async Task<SingleResponse<Student>> GetStudentByRegister(string register, string passcode)
        {
            try
            {
                passcode = passcode.EncryptPassword();

                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    Student student = await dataBase.Students.FirstOrDefaultAsync(c => c.Register == register && c.Passcode == passcode);
                    if (student == null)
                    {
                        return ResponseMessage.CreateNotFoundData<Student>();
                    }
                    //Precisa de cookies
                    return ResponseMessage.CreateSingleSuccessResponse<Student>(student);
                }
            }
            catch (Exception ex)
            {
                SingleResponse<Student> student = (SingleResponse<Student>)ResponseMessage.CreateErrorResponse(ex);
                return student;
            }
        }
    }
}
