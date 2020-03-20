using System.Threading.Tasks;

namespace Angus.Bills.CQRS.Commands
{
    public interface ICommandDispatcher
    {
        Task SendAsync<T>(T command) where T : class, ICommand;
    }
}