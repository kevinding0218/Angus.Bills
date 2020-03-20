using System.Threading.Tasks;

namespace Angus.Bills.CQRS.Commands
{
    public interface ICommandHandler<in TCommand> where TCommand : class, ICommand
    {
        Task HandleAsync(TCommand command);
    }
}