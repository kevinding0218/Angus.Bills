using System.Threading.Tasks;
using Drex.Types.Queries;

namespace Drex.CQRS.Events
{
    public interface IQueryHandler<in TQuery, TResult> where TQuery : class, IQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}