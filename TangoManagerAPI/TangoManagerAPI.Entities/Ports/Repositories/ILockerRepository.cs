using System.Threading.Tasks;
using TangoManagerAPI.Entities.Models;

namespace TangoManagerAPI.Entities.Ports.Repositories
{
    public interface ILockerRepository
    {
        Task<PacketLockEntity> CreateLockerAsync(string packetName);
    }
}
