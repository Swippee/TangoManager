using System.Threading.Tasks;
using TangoManagerAPI.Entities.Models;
using TangoManagerAPI.Entities.Ports.Routers;

namespace TangoManagerAPI.Entities.Commands.CommandsPaquet
{
    public sealed class CreatePaquetCommand : ACommand<PaquetEntity, CreatePaquetCommand>
    {
        public string Name { get; }
        public string Description { get; }


        public CreatePaquetCommand(string name, string description)
        {
            Name = name;
            Description = description;
          
        }

        public override async Task<PaquetEntity> ExecuteAsync(ICommandRouter commandRouter)
        {
            return await commandRouter.RouteAwaitForResultAsync(this);
        }
    }
}