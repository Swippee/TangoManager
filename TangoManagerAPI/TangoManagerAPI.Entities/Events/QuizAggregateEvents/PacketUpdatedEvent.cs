using System.Threading.Tasks;
using TangoManagerAPI.Entities.Models;
using TangoManagerAPI.Entities.Ports.Routers;

namespace TangoManagerAPI.Entities.Events.QuizAggregateEvents
{
    public sealed class PacketUpdatedEvent : AEvent<PaquetEntity>
    {
        public PacketUpdatedEvent(PaquetEntity data) : base(data.Clone())
        {

        }

        public override async Task DispatchAsync(IEventRouter eventRouter)
        {
            await eventRouter.RouteAsync(this);
        }
    }
}
