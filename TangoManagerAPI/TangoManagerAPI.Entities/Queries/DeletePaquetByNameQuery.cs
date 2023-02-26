using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TangoManagerAPI.Entities.Ports.Router;
using TangoManagerAPI.Models;

namespace TangoManagerAPI.Entities.Queries
{
    public sealed class DeletePaquetByNameQuery : AQuery<DeletePaquetByNameQuery>
    {
        public string Name { get;}
        public DeletePaquetByNameQuery(string name)
        {
            Name = name;
        }

        public override async Task QueryAsync(IQueryRouter queryRouter)
        {
            await queryRouter.RouteAsync(this);
        }
    }
}
