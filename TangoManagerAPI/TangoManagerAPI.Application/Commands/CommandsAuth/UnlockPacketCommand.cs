using TangoManagerAPI.Entities.Commands;
using TangoManagerAPI.Entities.Ports.Routers;

namespace TangoManagerAPI.Application.Commands.CommandsAuth
{
    public sealed class UnlockPacketCommand : ACommand<Task, UnlockPacketCommand>
    {
        public string PacketName { get; }
        public string LockToken { get; }

        public UnlockPacketCommand(string packetName, string lockToken)
        {
            PacketName = packetName;
            LockToken = lockToken;
        }

        public override async Task<Task> ExecuteAsync(ICommandRouter commandRouter)
        {
            return await commandRouter.RouteAwaitForResultAsync(this);
        }
    }
}
