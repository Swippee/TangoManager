using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TangoManagerAPI.Entities.Models;
using TangoManagerAPI.Entities.Ports.Handler;
using TangoManagerAPI.Entities.Ports.Repositories;
using TangoManagerAPI.Entities.Queries;

namespace TangoManagerAPI.Infrastructures.Handlers
{
    public class QueryHandler :
       IQueryHandler<IEnumerable<PaquetEntity>, GetAllPaquetsQuery>

    {
        private readonly IPaquetRepository _paquetRepository;

        public QueryHandler(IPaquetRepository paquetRepository)
        {
            _paquetRepository = paquetRepository;
        }

        public async Task<IEnumerable<PaquetEntity>> HandleAsync(GetAllPaquetsQuery query, CancellationToken token = default)
        {
            return await _paquetRepository.GetPaquetsAsync();
        }
    }
}
