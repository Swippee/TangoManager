#nullable enable
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using TangoManagerAPI.Application.Queries.CommandsAuth;
using TangoManagerAPI.Entities.Models;
using TangoManagerAPI.Entities.Ports.Handler;
using TangoManagerAPI.Entities.Ports.Repositories;
using TangoManagerAPI.Entities.Queries;

namespace TangoManagerAPI.Infrastructures.Handlers
{
    public class QueryHandler :
       IQueryHandler<IEnumerable<PacketAggregate>, GetAllPaquetsQuery>,
       IQueryHandler<PacketLockEntity?, GetPacketLockQuery>

    {
        private readonly IPaquetRepository _packetRepository;
        private readonly IMemoryCache _memoryCache;

        public QueryHandler(IPaquetRepository packetRepository, IMemoryCache memoryCache)
        {
            _packetRepository = packetRepository;
            _memoryCache = memoryCache;
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
                    packetLockEntity.UpdateLastAccessedDateTime();
                    return packetLockEntity;
                }
            }

            //TODO
            //Look into repository
            //implement packet repository
            //packetLockEntity = _packetLockRepository.GetAsync();

            if (packetLockEntity == null) 
                return null;

            //Save into cache if found
            packetLockEntity.UpdateLastAccessedDateTime();
            _memoryCache.Set(query.PacketName, packetLockEntity, new MemoryCacheEntryOptions
            {
                SlidingExpiration = PacketLockEntity.CacheExpiration
            });

            return packetLockEntity;
        }
    }
}
