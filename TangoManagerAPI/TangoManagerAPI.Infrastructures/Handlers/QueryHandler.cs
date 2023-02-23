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
       IQueryHandler<IEnumerable<PaquetEntity>, AQueryPaquet>

    {
        private readonly IPaquetRepository _paquetRepository;

        public QueryHandler(IPaquetRepository paquetRepository)
        {
            _paquetRepository = paquetRepository;
        }

        public async Task<IEnumerable<PaquetEntity>> HandleAsync(AQueryPaquet query, CancellationToken token = default)
        {
            return await _paquetRepository.GetPaquetsAsync();
        }
    }
}
