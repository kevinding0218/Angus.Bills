using System.Threading.Tasks;
using MongoDB.Driver;

namespace Drex.Persistence.MongoDB
{
    public interface IMongoSessionFactory
    {
        Task<IClientSessionHandle> CreateAsync();
    }
}