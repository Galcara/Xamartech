using Common;
using Metadata;
using System.Threading.Tasks;

namespace BusinessLogicalLayer.Interfaces
{
    public interface IAdministratorService : IPictureMatadataCRUD<Administrator>
    {
        Task<SingleResponse<Administrator>> GetAdmByEmail(string email, string passcode);
        Task<SingleResponse<Administrator>> ChangeStatus(int id);


    }
}
