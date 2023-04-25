#nullable enable
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using TangoManagerAPI.Application.Queries.CommandsAuth;
using TangoManagerAPI.Entities.Models;
using TangoManagerAPI.Entities.Ports.Handler;
using TangoManagerAPI.Entities.Ports.Repositories;
using TangoManagerAPI.Entities.Queries;
using TangoManagerAPI.Infrastructures.Repositories;

namespace TangoManagerAPI.Infrastructures.Handlers
{
    public class QueryHandler :
       IQueryHandler<IEnumerable<PacketAggregate>, GetAllPaquetsQuery>,
       IQueryHandler<PacketLockEntity?, GetPacketLockQuery>

    {
        private readonly IPaquetRepository _packetRepository;
        private readonly IPacketLockRepository _packetLockRepository;
        private readonly IMemoryCache _memoryCache;

        public QueryHandler(IPaquetRepository packetRepository, IMemoryCache memoryCache, IPacketLockRepository packetLockRepository)
        {
            _packetRepository = packetRepository;
            _memoryCache = memoryCache;
            _packetLockRepository = packetLockRepository;
        }

        public async Task<IEnumerable<PacketAggregate>> HandleAsync(GetAllPaquetsQuery query, CancellationToken token = default)
        {
            return await _packetRepository.GetPacketsAsync();
        }

        public async Task<PacketLockEntity?> HandleAsync(GetPacketLockQuery query, CancellationToken token = default)
        {
            //Look into cache first
            if (_memoryCache.TryGetValue(query.PacketName, out PacketLockEntity? packetLockEntity))
            {
                if (packetLockEntity != null)
                {
                    return packetLockEntity;
                }
            }

            //TODO
             packetLockEntity = await _packetLockRepository.GetAsync(query.PacketName);

            if (packetLockEntity == null) 
                return null;

            //Save into cache if found
            _memoryCache.Set(query.PacketName, packetLockEntity, new MemoryCacheEntryOptions
            {
                SlidingExpiration = PacketLockEntity.CacheExpiration
            });

            return packetLockEntity;
        }
    }
}
