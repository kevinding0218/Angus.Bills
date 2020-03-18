using System.Threading.Tasks;
using MongoDB.Driver;

namespace Angus.Bills.Persistence.MongoDB
{
    public interface IMongoSessionFactory
    {
        Task<IClientSessionHandle> CreateAsync();
    }
}