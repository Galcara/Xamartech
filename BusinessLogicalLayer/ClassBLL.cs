using BusinessLogicalLayer.Interfaces;
using Common;
using DataAccessLayer;
using Metadata;
using Metadata.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogicalLayer
{
    public class ClassBLL : BaseValidator<Class>, IClassService
    {
        public override Response Validate(Class clasS)
        {
            AddError(clasS.ClassName.IsValidName());
            if (!Enum.IsDefined(typeof(Shift), clasS.ClassShift))
            {
                AddError("Escolha um turno válido.");
            }


            return base.Validate(clasS);
        }

        public async Task<Response> Insert(Class clasS)
        {
            Response response = Validate(clasS);
            if (response.Success)
            {
                clasS.ClassName = clasS.ClassName.ToUpper();
                try
                {
                    using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                    {
                        Class @class = await dataBase.Classes.FirstOrDefaultAsync(m => m.ClassName == clasS.ClassName);
                        if (@class != null)
                        {
                            return ResponseMessage.CreateDuplicateErrorResponse();
                        }
                        dataBase.Classes.Add(clasS);
                        await dataBase.SaveChangesAsync();
                        ResponseMessage.CreateSuccessResponse();
                    }
                }
                catch (Exception ex)
                {
                    return ResponseMessage.CreateErrorResponse(ex);
                }
            }
            return response;
        }

        public async Task<Response> Update(Class clasS)
        {
            Response validationResponse = Validate(clasS);
            if (!validationResponse.Success)
            {
                return validationResponse;
            }
            try
            {
                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    dataBase.Entry(clasS).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    await dataBase.SaveChangesAsync();
                }
                return ResponseMessage.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                return ResponseMessage.CreateErrorResponse(ex);
            }
        }

        public async Task<SingleResponse<Class>> GetByID(int id)
        {
            try
            {
                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    Class clasS = await dataBase.Classes.FirstOrDefaultAsync(p => p.ID == id);
                    if (clasS == null)
                    {
                        return ResponseMessage.CreateNotFoundData<Class>();
                    }
                    return ResponseMessage.CreateSingleSuccessResponse<Class>(clasS);
                }
            }
            catch (Exception ex)
            {
                SingleResponse<Class> clasS = (SingleResponse<Class>)ResponseMessage.CreateErrorResponse(ex);
                return clasS;
            }


        }

        public async Task<QueryResponse<Class>> GetAll()
        {
            try
            {
                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    List<Class> classes = await dataBase.Classes.ToListAsync();
                    return ResponseMessage.CreateQuerySuccessResponse<Class>(classes);
                }
            }
            catch (Exception ex)
            {
                QueryResponse<Class> clasS = (QueryResponse<Class>)ResponseMessage.CreateErrorResponse(ex);
                return clasS;
            }
        }
    }
}

