using System.Threading.Tasks;
using TangoManagerAPI.Entities.Models;
using TangoManagerAPI.Entities.Ports.Routers;

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
