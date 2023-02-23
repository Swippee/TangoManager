using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TangoManagerAPI.Entities.Ports.Router;
using TangoManagerAPI.Models;

namespace TangoManagerAPI.Entities.Commands
{
    public class CreateNewPaquetCommand : ACommand<PaquetEntity,CreateNewPaquetCommand>
    {
        public string Name { get; }
        public string Description { get; }
        public CreateNewPaquetCommand(string name, string description) 
        {
            this.Name = name;
            this.Description= description;
        }

        public override async Task<PaquetEntity> ExecuteAsync(ICommandRouter commandRouter)
        {
            return await commandRouter.RouteAwaitForResultAsync(this);
        }
    }
}
