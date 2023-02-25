using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TangoManagerAPI.Entities.Ports.Routers;

namespace TangoManagerAPI.Entities.Commands
{
    public abstract class ACommand<TResult, TCommand> : ACommand
    {
        public override Type CommandType => typeof(TCommand);

        public abstract Task<TResult> ExecuteAsync(ICommandRouter commandRouter);
    }

    public abstract class ACommand<TCommand> : ACommand
    {
        public override Type CommandType => typeof(TCommand);

        public abstract Task ExecuteAsync(ICommandRouter commandRouter);
    }

    public abstract class ACommand
    {
        public abstract Type CommandType { get; }
    }
}
