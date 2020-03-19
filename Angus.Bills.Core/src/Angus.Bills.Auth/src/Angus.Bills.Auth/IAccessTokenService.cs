using System.Threading.Tasks;

namespace Angus.Bills.Auth
{
    public interface IAccessTokenService
    {
        Task<bool> IsCurrentActiveToken();
        Task DeactivateCurrentAsync();
        Task<bool> IsActiveAsync(string token);
        Task DeactivateAsync(string token);
    }
}