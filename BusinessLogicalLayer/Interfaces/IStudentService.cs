using Common;
using Metadata;
using System.Threading.Tasks;

namespace BusinessLogicalLayer.Interfaces
{
    public interface IStudentService : IPictureMatadataCRUD<Student>
    {
        Task<SingleResponse<Student>> IsUniqueRegister(string register);
        Task<SingleResponse<Student>> ChangeStatus(int id);
        Task<SingleResponse<Student>> GetStudentByRegister(string register, string passcode);
    }
}