using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangoManagerAPI.Entities.Models;
using TangoManagerAPI.Entities.Ports.Routers;

namespace TangoManagerAPI.Entities.Commands.CommandsLock
{
    public  class AddLockToPacketCommand : ACommand<PacketLockEntity, AddLockToPacketCommand>
    {
        public string PacketName { get; set; }
        public AddLockToPacketCommand(string packetName)
        {
            PacketName= packetName;
        }
        public override async Task<PacketLockEntity> ExecuteAsync(ICommandRouter commandRouter)
        {
            return await commandRouter.RouteAwaitForResultAsync(this);
        }
    }
}
