using System.Threading.Tasks;
using Angus.Bills.Types.Queries;

namespace Angus.Bills.CQRS.Events
{
    public interface IQueryDispatcher
    {
        Task<TResult> QueryAsync<TResult>(IQuery<TResult> query);
        Task<TResult> QueryAsync<TQuery, TResult>(TQuery query) where TQuery : class, IQuery<TResult>;
    }
}