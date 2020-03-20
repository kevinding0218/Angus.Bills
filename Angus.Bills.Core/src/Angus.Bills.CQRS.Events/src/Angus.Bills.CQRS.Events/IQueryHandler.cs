using System.Threading.Tasks;
using Angus.Bills.Types.Queries;

namespace Angus.Bills.CQRS.Events
{
    public interface IQueryHandler<in TQuery, TResult> where TQuery : class, IQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}