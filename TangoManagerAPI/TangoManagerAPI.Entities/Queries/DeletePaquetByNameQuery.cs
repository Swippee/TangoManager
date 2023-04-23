using System;
using System.Threading.Tasks;
using TangoManagerAPI.Entities.Ports.Routers;

namespace TangoManagerAPI.Entities.Queries
{
    public sealed class DeletePaquetByNameQuery : AQuery<DeletePaquetByNameQuery>
    {
        public string Name { get;}
        public DeletePaquetByNameQuery(string name)
        {
            Name = name;
        }


        public override Task QueryAsync(IQueryRouter queryRouter)
        {
            throw new NotImplementedException();
        }
    }
}
