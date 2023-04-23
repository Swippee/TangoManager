using TangoManagerAPI.Entities.Commands;
using TangoManagerAPI.Entities.Models;
using TangoManagerAPI.Entities.Ports.Routers;

namespace TangoManagerAPI.Application.Commands.CommandsAuth
{
    public sealed class LockPacketCommand : ACommand<PacketLockEntity, LockPacketCommand>
    {
        public string PacketName { get; }

        public LockPacketCommand(string packetName)
        {
            PacketName = packetName;
        }

        public override async Task<PacketLockEntity> ExecuteAsync(ICommandRouter commandRouter)
        {
            return await commandRouter.RouteAwaitForResultAsync(this);
        }
    }
}
