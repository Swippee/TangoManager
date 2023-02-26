using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TangoManagerAPI.Entities.Ports.Handler;
using TangoManagerAPI.Entities.Ports.Repository;
using TangoManagerAPI.Entities.Queries;
using TangoManagerAPI.Models;

namespace TangoManagerAPI.Infrastructures.Handlers
{
    public class QueryHandler :
       IQueryHandler<IEnumerable<PaquetEntity>, GetPaquetsQuery>,
       IQueryHandler<PaquetEntity, GetPaquetByNameQuery>,
       IQueryHandler<DeletePaquetByNameQuery>
    {
        private readonly IPaquetRepository _paquetRepository;

        public QueryHandler(IPaquetRepository paquetRepository)
        {
            _paquetRepository = paquetRepository;
        }

        public async Task<IEnumerable<PaquetEntity>> HandleAsync(GetPaquetsQuery query, CancellationToken token = default)
        {
            return await _paquetRepository.GetPaquetsAsync();
        }

        public async Task<PaquetEntity> HandleAsync(GetPaquetByNameQuery query, CancellationToken token = default)
        {
            return await _paquetRepository.GetPaquetByNameAsync(query.Name);
        }

        public async Task HandleAsync(DeletePaquetByNameQuery query, CancellationToken token = default)
        {
            await _paquetRepository.RemovePaquetAsync(query.Name);
        }
    }
}
