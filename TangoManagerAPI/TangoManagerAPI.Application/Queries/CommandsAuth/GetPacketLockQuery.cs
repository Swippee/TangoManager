using TangoManagerAPI.Entities.Models;
using TangoManagerAPI.Entities.Ports.Routers;
using TangoManagerAPI.Entities.Queries;

namespace TangoManagerAPI.Application.Queries.CommandsAuth
{
    public sealed class GetPacketLockQuery : AQuery<PacketLockEntity?, GetPacketLockQuery>
    {
        public string PacketName { get; }

        public GetPacketLockQuery(string packetName)
        {
            PacketName = packetName;
        }

        public override async Task<PacketLockEntity?> QueryAsync(IQueryRouter queryRouter)
        {
            return await queryRouter.RouteAsync(this);
        }
    }
}
