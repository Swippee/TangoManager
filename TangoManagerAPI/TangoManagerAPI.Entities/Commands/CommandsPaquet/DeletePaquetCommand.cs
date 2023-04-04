using System.Threading.Tasks;
using TangoManagerAPI.Entities.Models;
using TangoManagerAPI.Entities.Ports.Routers;

namespace TangoManagerAPI.Entities.Commands.CommandsPaquet
{
    public sealed class DeletePaquetCommand : ACommand<PacketAggregate, DeletePaquetCommand>
    {
        public string Name { get; }


        public DeletePaquetCommand(string name )
        {
            Name = name;
        }

        public override async Task<PacketAggregate> ExecuteAsync(ICommandRouter commandRouter)
        {
            return await commandRouter.RouteAwaitForResultAsync(this);
        }
    }
}