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
    public class SubjectBLL : BaseValidator<Subject>, ISubjectService
    {
        public override Response Validate(Subject subject)
        {
            AddError(subject.SubjectName.IsValidName());
            return base.Validate(subject);
        }

        public async Task<Response> Insert(Subject subject)
        {
            Response response = Validate(subject);
            if (response.Success)
            {
                subject.SubjectName = subject.SubjectName.ToUpper();

                try
                {
                    using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                    {
                        Subject sub = await dataBase.Subjects.FirstOrDefaultAsync(m => m.SubjectName == subject.SubjectName);
                        if (sub != null)
                        {
                            return ResponseMessage.CreateDuplicateErrorResponse();
                        }
                        dataBase.Subjects.Add(subject);
                        await dataBase.SaveChangesAsync();
                        return ResponseMessage.CreateSuccessResponse();
                    }
                }
                catch (Exception ex)
                {
                    return ResponseMessage.CreateErrorResponse(ex);
                }
            }
            return response;
        }

        public async Task<Response> Update(Subject subject)
        {
            Response validationResponse = Validate(subject);
            if (!validationResponse.Success)
            {
                return validationResponse;
            }
            try
            {
                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    dataBase.Entry(subject).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    await dataBase.SaveChangesAsync();
                }
                return ResponseMessage.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                return ResponseMessage.CreateErrorResponse(ex);
            }
        }

        public async Task<QueryResponse<Subject>> GetAll()
        {
            try
            {
                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    List<Subject> subjects = await dataBase.Subjects.Include(c => c.Teachers).ToListAsync();
                    return ResponseMessage.CreateQuerySuccessResponse<Subject>(subjects);
                }
            }
            catch (Exception ex)
            {
                QueryResponse<Subject> subject = (QueryResponse<Subject>)ResponseMessage.CreateErrorResponse(ex);
                return subject;
            }
        }

        public async Task<SingleResponse<Subject>> GetByID(int id)
        {
            try
            {
                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    Subject subject = await dataBase.Subjects.FirstOrDefaultAsync(p => p.ID == id);
                    if (subject == null)
                    {
                        return ResponseMessage.CreateNotFoundData<Subject>();
                    }
                    return ResponseMessage.CreateSingleSuccessResponse<Subject>(subject);
                }
            }
            catch (Exception ex)
            {
                SingleResponse<Subject> subject = (SingleResponse<Subject>)ResponseMessage.CreateErrorResponse(ex);
                return subject;
            }
        }

        public async Task<SingleResponse<Subject>> ChangeStatus(int id)
        {
            try
            {
                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    Subject subject = dataBase.Subjects.Find(id);
                    if (subject.Active == false)
                    {
                        subject.Active = true;
                    }
                    else
                    {
                        subject.Active = false;
                    }
                    await dataBase.SaveChangesAsync();
                    return ResponseMessage.CreateSingleSuccessResponse<Subject>(subject);
                }
            }
            catch (Exception ex)
            {
                ResponseMessage.CreateErrorResponse(ex);
                return ResponseMessage.CreateNotFoundData<Subject>();
            }
        }
    }
}
