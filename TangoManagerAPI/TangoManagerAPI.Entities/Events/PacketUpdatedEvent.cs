using TangoManagerAPI.Entities.Models;
using TangoManagerAPI.Entities.Ports.Routers;

namespace TangoManagerAPI.Entities.Events
{
    public sealed class PacketUpdatedEvent : AEvent<PaquetEntity>
    {
        public PacketUpdatedEvent(PaquetEntity data) : base(data.Clone())
        {

        }

        public override async void Dispatch(IEventRouter eventRouter)
        {
            await eventRouter.RouteAsync(this);
        }
    }
}
