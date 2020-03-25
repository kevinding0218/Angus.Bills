using System.Threading.Tasks;

namespace Drex.Initializers
{
    public interface IInitializer
    {
        Task InitializeAsync();
    }
}