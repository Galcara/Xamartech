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
    public class AdministratorBLL : BaseValidator<Administrator>, IAdministratorService
    {
        public override Response Validate(Administrator administrator)
        {
            AddError(administrator.AdmName.IsValidName());
            AddError(administrator.Cpf.IsValidCPF());
            AddError(administrator.Email.IsValidEmail());
            AddError(administrator.PhoneNumber.IsValidPhoneNumber());
            AddError(administrator.Passcode.IsValidPasscode());
            return base.Validate(administrator);
        }

        public async Task<SingleResponse<int>> Insert(Administrator administrator)
        {
            Response response = Validate(administrator);
            if (response.Success)
            {
                administrator.Cpf = administrator.Cpf.RemoveMaskCPF();
                administrator.PhoneNumber = administrator.PhoneNumber.RemoveMaskPhoneNumber();
                administrator.Passcode = administrator.Passcode.EncryptPassword();
                administrator.Active = true;
                try
                {
                    using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                    {

                        dataBase.Administrators.Add(administrator);
                        await dataBase.SaveChangesAsync();
                        SingleResponse<int> data = new SingleResponse<int>();
                        data.Data = administrator.ID;
                        data.Success = true;
                        data.Message = "Cadastrado com sucesso.";
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

        public async Task<Response> Update(Administrator administrator)
        {

            Response validationResponse = Validate(administrator);
            if (!validationResponse.Success)
            {
                return validationResponse;
            }
            try
            {
                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    dataBase.Entry(administrator).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    await dataBase.SaveChangesAsync();
                }
                return ResponseMessage.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                return ResponseMessage.CreateErrorResponse(ex);
            }

        }

        public async Task<SingleResponse<Administrator>> GetByID(int id)
        {
            try
            {
                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    Administrator administrator = await dataBase.Administrators.FirstOrDefaultAsync(p => p.ID == id);
                    if (administrator == null)
                    {
                        return ResponseMessage.CreateNotFoundData<Administrator>();
                    }
                    return ResponseMessage.CreateSingleSuccessResponse<Administrator>(administrator);
                }
            }
            catch (Exception ex)
            {
                SingleResponse<Administrator> administrator = (SingleResponse<Administrator>)ResponseMessage.CreateErrorResponse(ex);
                return administrator;
            }

        }

        public async Task<QueryResponse<Administrator>> GetAll()
        {
            try
            {
                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    List<Administrator> administrators = await dataBase.Administrators.ToListAsync();
                    return ResponseMessage.CreateQuerySuccessResponse<Administrator>(administrators);
                }
            }
            catch (Exception ex)
            {
                QueryResponse<Administrator> administrator = (QueryResponse<Administrator>)ResponseMessage.CreateErrorResponse(ex);
                return administrator;
            }

        }

        public async Task<SingleResponse<Administrator>> GetAdmByEmail(string email, string passcode)
        {
            try
            {
                passcode = passcode.EncryptPassword();

                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    Administrator administrator = await dataBase.Administrators.FirstOrDefaultAsync(c => c.Email == email && c.Passcode == passcode);
                    if (administrator == null)
                    {
                        return ResponseMessage.CreateNotFoundData<Administrator>();
                    }
                    //Precisa de cookies
                    return ResponseMessage.CreateSingleSuccessResponse<Administrator>(administrator);
                }
            }
            catch (Exception ex)
            {
                SingleResponse<Administrator> administrator = (SingleResponse<Administrator>)ResponseMessage.CreateErrorResponse(ex);
                return administrator;
            }
        }

        public async Task<SingleResponse<Administrator>> ChangeStatus(int id)
        {
            try
            {
                using (BiometricPresenceDB dataBase = new BiometricPresenceDB())
                {
                    Administrator administrator = dataBase.Administrators.Find(id);
                    if (administrator.Active == false)
                    {
                        administrator.Active = true;
                    }
                    else
                    {
                        administrator.Active = false;
                    }
                    await dataBase.SaveChangesAsync();
                    return ResponseMessage.CreateSingleSuccessResponse<Administrator>(administrator);
                }
            }
            catch (Exception ex)
            {
                ResponseMessage.CreateErrorResponse(ex);
                return ResponseMessage.CreateNotFoundData<Administrator>();
            }

        }

    }
}