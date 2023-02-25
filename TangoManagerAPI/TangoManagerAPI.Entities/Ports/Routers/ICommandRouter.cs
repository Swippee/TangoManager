using System.Threading.Tasks;
using TangoManagerAPI.Entities.Commands;

namespace TangoManagerAPI.Entities.Ports.Routers;

public interface ICommandRouter
{
    public Task<TResult> RouteAwaitForResultAsync<TResult, TCommand>(ACommand<TResult, TCommand> command) where TCommand : ACommand<TResult, TCommand>;
    public Task RouteAsync<TCommand>(TCommand command) where TCommand : ACommand<TCommand>;
}