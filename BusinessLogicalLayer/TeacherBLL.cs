using BusinessLogicalLayer.Interfaces;
using Common;
using DataAccessLayer;
using Metadata;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogicalLayer
{
    public class TeacherBLL : BaseValidator<Teacher>, ITeacherService
    {
        public override Response Validate(Teacher teacher)
        {
            AddError(teacher.TeacherName.IsValidName());
            AddError(teacher.Cpf.IsValidCPF());
            AddError(teacher.Email.IsValidEmail());
            AddError(teacher.PhoneNumber.IsValidPhoneNumber());
            return base.Validate(teacher);
        }

        public async Task<SingleResponse<int>> Insert(Teacher teacher)
        {
            Response response = Validate(teacher);
            if (response.Success)
            {
                teacher.Cpf = teacher.Cpf.RemoveMaskCPF();
                teacher.PhoneNumber = teacher.PhoneNumber.RemoveMaskPhoneNumber();
                teacher.Passcode = teacher.Cpf;
                teacher.Passcode = teacher.Passcode.EncryptPassword();
                teacher.Active = true;
                try
                {
                    using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                    {
                        dataBase.Subjects.AttachRange(teacher.Subjects);
                        dataBase.Teachers.Add(teacher);
                        await dataBase.SaveChangesAsync();
                        SingleResponse<int> data = new SingleResponse<int>();
                        data.Data = teacher.ID;
                        data.Success = true;
                        data.Message = "Professor cadastrado com sucesso.";
                        return data;
                    }
                }
                catch (Exception)
                {
                    return ResponseMessage.CreateSingleErrorResponse<int>();
                }
            }
            return ResponseMessage.CreateSingleErrorResponse<int>();
        }

        public async Task<Response> Update(Teacher teacher)
        {
            Response validationResponse = Validate(teacher);
            if (!validationResponse.Success)
            {
                return validationResponse;
            }
            try
            {
                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    dataBase.Entry(teacher).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    await dataBase.SaveChangesAsync();
                }
                return ResponseMessage.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                return ResponseMessage.CreateErrorResponse(ex);
            }
        }

        public async Task<SingleResponse<Teacher>> GetByID(int id)
        {
            try
            {
                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    Teacher teacher = await dataBase.Teachers.FirstOrDefaultAsync(p => p.ID == id);
                    if (teacher == null)
                    {
                        return ResponseMessage.CreateNotFoundData<Teacher>();
                    }
                    return ResponseMessage.CreateSingleSuccessResponse<Teacher>(teacher);
                }
            }
            catch (Exception ex)
            {
                SingleResponse<Teacher> teacher = (SingleResponse<Teacher>)ResponseMessage.CreateErrorResponse(ex);
                return teacher;
            }
        }

        public async Task<QueryResponse<Teacher>> GetAll()
        {
            try
            {
                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    List<Teacher> teachers = await dataBase.Teachers.Include(c => c.Subjects).ToListAsync();
                    return ResponseMessage.CreateQuerySuccessResponse<Teacher>(teachers);
                }
            }
            catch (Exception ex)
            {
                QueryResponse<Teacher> teacher = (QueryResponse<Teacher>)ResponseMessage.CreateErrorResponse(ex);
                return teacher;
            }
        }

        public async Task<SingleResponse<Teacher>> GetTeacherByEmail(string email, string passcode)
        {
            try
            {
                passcode = passcode.EncryptPassword();

                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    Teacher teacher = await dataBase.Teachers.FirstOrDefaultAsync(c => c.Email == email && c.Passcode == passcode);
                    if (teacher == null)
                    {
                        return ResponseMessage.CreateNotFoundData<Teacher>();
                    }
                    //Precisa de cookies
                    return ResponseMessage.CreateSingleSuccessResponse<Teacher>(teacher);
                }
            }
            catch (Exception ex)
            {
                SingleResponse<Teacher> teacher = (SingleResponse<Teacher>)ResponseMessage.CreateErrorResponse(ex);
                return teacher;
            }
        }

        public async Task<SingleResponse<Teacher>> ChangeStatus(int id)
        {
            try
            {
                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    Teacher teacher = dataBase.Teachers.Find(id);
                    if (teacher.Active == false)
                    {
                        teacher.Active = true;
                    }
                    else
                    {
                        teacher.Active = false;
                    }
                    await dataBase.SaveChangesAsync();
                    return ResponseMessage.CreateSingleSuccessResponse<Teacher>(teacher);
                }
            }
            catch (Exception ex)
            {
                ResponseMessage.CreateErrorResponse(ex);
                return ResponseMessage.CreateNotFoundData<Teacher>();

            }
        }

        public async Task<QueryResponse<Presence>> GetPresenceListOfLesson(int id)
        {
            using (BiometricPresenceDB db = new BiometricPresenceDB())
            {
                List<Presence> presences = await db.Presences.Include(c=> c.Student).Where(c => c.LessonID == id).ToListAsync();
                return ResponseMessage.CreateQuerySuccessResponse<Presence>(presences);
            }
        }

        public async Task<QueryResponse<Lesson>> GetLessonsByTeacher(int id)
        {
            using (BiometricPresenceDB db = new BiometricPresenceDB())
            {
                List<Lesson> lessons = await db.Lessons.Include(c => c.Class).Include(c=>c.Subject).Where(c=> c.TeacherID == id).ToListAsync();
                return ResponseMessage.CreateQuerySuccessResponse<Lesson>(lessons);
            }
        }
    }
}