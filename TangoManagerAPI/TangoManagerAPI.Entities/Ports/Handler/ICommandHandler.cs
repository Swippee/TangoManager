using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TangoManagerAPI.Entities.Commands;

namespace TangoManagerAPI.Entities.Ports.Handler
{
    public interface ICommandHandler<TResult, in TCommand> : ICommandHandler where TCommand : ACommand
    {
        public Type SupportedCommandType => typeof(TCommand);

        Task<TResult> HandleAsync(TCommand command);
    }

    public interface ICommandHandler<in TCommand> : ICommandHandler where TCommand : ACommand
    {
        Type SupportedCommandType => typeof(TCommand);

        void Handle(TCommand command);
    }

    public interface ICommandHandler
    {
        Type GetSupportedCommandType<TCommand>() where TCommand : ACommand => typeof(TCommand);
    }
}
