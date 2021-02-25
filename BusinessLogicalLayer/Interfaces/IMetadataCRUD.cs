using Common;
using System.Threading.Tasks;

namespace BusinessLogicalLayer.Interfaces
{
    public interface IMetadataCRUD<T>
    {
        Task<Response> Insert(T item);
        Task<Response> Update(T item);
        Task<SingleResponse<T>> GetByID(int id);
        Task<QueryResponse<T>> GetAll();

    }
}
