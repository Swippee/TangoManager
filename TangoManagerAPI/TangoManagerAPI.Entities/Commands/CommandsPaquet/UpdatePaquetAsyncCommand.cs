using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TangoManagerAPI.Entities.Ports.Router;
using TangoManagerAPI.Models;

namespace TangoManagerAPI.Entities.Commands.CommandsPaquet
{
    public class UpdatePaquetAsyncCommand : ACommand<PaquetEntity, UpdatePaquetAsyncCommand>
    {
        public string Name { get; }
        public string Description { get; }
        public UpdatePaquetAsyncCommand(string name, string description)
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
