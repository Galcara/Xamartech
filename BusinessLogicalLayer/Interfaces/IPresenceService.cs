using Common;
using Metadata;
using System.Threading.Tasks;

namespace BusinessLogicalLayer.Interfaces
{
    public interface IPresenceService 
    {
        Task<Response> ApplyFrequency(string register);
    }
}
