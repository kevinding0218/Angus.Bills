using System.Threading.Tasks;

namespace Drex.WebApi.Requests
{
    public interface IRequestDispatcher
    {
        Task<TResult> DispatchAsync<TRequest, TResult>(TRequest request) where TRequest : class, IRequest;
    }
}