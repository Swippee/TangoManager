using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TangoManagerAPI.Entities.Models;
using TangoManagerAPI.Entities.Ports.Routers;

namespace TangoManagerAPI.Entities.Queries
{
    public sealed class GetAllPaquetsQuery : AQuery<IEnumerable<PaquetEntity>, GetAllPaquetsQuery>
    {
        public override async Task<IEnumerable<PaquetEntity>> QueryAsync(IQueryRouter queryRouter)
        {
            return await queryRouter.RouteAsync(this);
        }
    }
}
