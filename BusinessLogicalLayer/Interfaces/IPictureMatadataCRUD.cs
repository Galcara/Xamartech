using Common;
using System.Threading.Tasks;

namespace BusinessLogicalLayer.Interfaces
{
    public interface IPictureMatadataCRUD<T>
    {
        Task<SingleResponse<int>> Insert(T item);
        Task<Response> Update(T item);
        Task<SingleResponse<T>> GetByID(int id);
        Task<QueryResponse<T>> GetAll();
    }
}
