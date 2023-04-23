using TangoManagerAPI.Entities.Models;
using TangoManagerAPI.Entities.Ports.Routers;

namespace TangoManagerAPI.Entities.Events.PacketLockEntityEvents
{
    public sealed class PacketLockAccessedEvent :  AEvent<PacketLockEntity>
    {
        public PacketLockAccessedEvent(PacketLockEntity data) : base(data)
        {
        }

        public override async void Dispatch(IEventRouter eventRouter)
        {
            await eventRouter.RouteAsync(this);
        }
    }
}
