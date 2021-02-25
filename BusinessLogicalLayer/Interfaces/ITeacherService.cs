using Common;
using Metadata;
using System.Threading.Tasks;

namespace BusinessLogicalLayer.Interfaces
{
    public interface ITeacherService : IPictureMatadataCRUD<Teacher>
    {
        Task<SingleResponse<Teacher>> ChangeStatus(int id);
        Task<SingleResponse<Teacher>> GetTeacherByEmail(string email, string passcode);
        Task<QueryResponse<Presence>> GetPresenceListOfLesson(int id);
        Task<QueryResponse<Lesson>> GetLessonsByTeacher(int id);

    }
}
