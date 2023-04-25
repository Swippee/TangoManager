﻿#nullable enable
using System.Collections.Generic;
using System.Threading.Tasks;
using TangoManagerAPI.Entities.Models;

namespace TangoManagerAPI.Entities.Ports.Repositories;

public interface IPacketLockRepository
{

    Task CreatePacketLockAsync(PacketLockEntity packetLockAggregate);
    Task DeletePacketLockAsync(string packetName);
    Task UpdatePacketLockAsync(PacketLockEntity packetLockAggregate);
    Task<PacketLockEntity> GetAsync(string packetName);
}