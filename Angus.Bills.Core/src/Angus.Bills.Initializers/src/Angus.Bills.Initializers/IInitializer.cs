using System.Threading.Tasks;

namespace Angus.Bills.Initializers {
    public interface IInitializer {
        Task InitializeAsync ();
    }
}