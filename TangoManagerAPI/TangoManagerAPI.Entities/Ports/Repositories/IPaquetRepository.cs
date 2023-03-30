using System.Collections.Generic;
using System.Threading.Tasks;
using TangoManagerAPI.Entities.Models;

namespace TangoManagerAPI.Entities.Ports.Repositories;

public interface IPaquetRepository
{
    Task<IEnumerable<PaquetEntity>> GetPacketsAsync();
    Task<PacketAggregate?> GetPacketByNameAsync(string packetName);
    Task SavePacketAsync(PacketAggregate packetAggregate);
}