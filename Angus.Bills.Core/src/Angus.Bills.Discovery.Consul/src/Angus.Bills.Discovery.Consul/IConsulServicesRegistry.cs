using System.Threading.Tasks;
using Angus.Bills.Discovery.Consul.Models;

namespace Angus.Bills.Discovery.Consul
{
    public interface IConsulServicesRegistry
    {
        Task<ServiceAgent> GetAsync(string name);
    }
}