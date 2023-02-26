using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TangoManagerAPI.Entities.Ports.Router;
using TangoManagerAPI.Models;

namespace TangoManagerAPI.Entities.Queries
{
    public sealed class GetPaquetByNameQuery : AQuery<PaquetEntity, GetPaquetByNameQuery>
    {
        public string Name { get;}
        public GetPaquetByNameQuery(string name)
        {
            Name = name;
        }

        public override async Task<PaquetEntity> QueryAsync(IQueryRouter queryRouter)
        {
            return await queryRouter.RouteAsync(this);
        }
    }
}
