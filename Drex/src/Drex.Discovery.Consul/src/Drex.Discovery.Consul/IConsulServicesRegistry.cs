using System.Threading.Tasks;
using Drex.Discovery.Consul.Models;

namespace Drex.Discovery.Consul
{
    public interface IConsulServicesRegistry
    {
        Task<ServiceAgent> GetAsync(string name);
    }
}