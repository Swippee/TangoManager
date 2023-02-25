using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using TangoManagerAPI.Entities.Commands;
using TangoManagerAPI.Entities.Ports.Handler;
using TangoManagerAPI.Entities.Ports.Handlers;
using TangoManagerAPI.Entities.Ports.Routers;

namespace TangoManagerAPI.Infrastructures.Routers
{
    public sealed class CommandRouter : ICommandRouter
    {
        private readonly IDictionary<Type, ICommandHandler> _commandHandlers;

        public CommandRouter()
        {
            _commandHandlers = new Dictionary<Type, ICommandHandler>();
        }
        public async Task<TResult> RouteAwaitForResultAsync<TResult, TCommand>(ACommand<TResult, TCommand> command) where TCommand : ACommand<TResult, TCommand>
        {
            if (_commandHandlers.TryGetValue(command.CommandType, out var commandHandler))
                return await ((ICommandHandler<TResult, TCommand>)commandHandler).HandleAsync((TCommand)command);
            throw new KeyNotFoundException();
        }

        public Task RouteAsync<TCommand>(TCommand command) where TCommand : ACommand<TCommand>
        {
            if (_commandHandlers.TryGetValue(command.CommandType, out var commandHandler))
                ((ICommandHandler<TCommand>)commandHandler).Handle(command);
            else
                throw new KeyNotFoundException();

            return Task.CompletedTask;
        }

        public void AddCommandHandler<TCommand>(ICommandHandler handler) where TCommand : ACommand
        {
            _commandHandlers.Add(handler.GetSupportedCommandType<TCommand>(), handler);
        }
    }
}
