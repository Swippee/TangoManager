using TangoManagerAPI.Entities.Models;
using TangoManagerAPI.Entities.Ports.Routers;

namespace TangoManagerAPI.Entities.Events.QuizAggregateEvents
{
    public sealed class QuizCardEntityAddedEvent : AEvent<QuizCardEntity>
    {
        public QuizCardEntityAddedEvent(QuizCardEntity data) : base(data.Clone())
        {

        }

        public override async void Dispatch(IEventRouter eventRouter)
        {
            await eventRouter.RouteAsync(this);
        }
    }
}
