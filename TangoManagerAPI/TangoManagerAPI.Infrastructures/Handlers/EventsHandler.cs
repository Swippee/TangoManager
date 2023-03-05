using TangoManagerAPI.Entities.Events.QuizAggregateEvents;
using TangoManagerAPI.Entities.Ports.Handlers;

namespace TangoManagerAPI.Infrastructures.Handlers
{
    public class EventsHandler : 
        IEventHandler<QuizAnsweredEvent>, 
        IEventHandler<QuizCardEntityAddedEvent>,
        IEventHandler<PacketUpdatedEvent>,
        IEventHandler<CardUpdatedEvent>
    {

        public EventsHandler()
        {

        }

        public async void Handle(QuizAnsweredEvent @event)
        {
           
        }

        public async void Handle(QuizCardEntityAddedEvent @event)
        {
          
        }

        public async void Handle(PacketUpdatedEvent @event)
        {
           
        }

        public async void Handle(CardUpdatedEvent @event)
        {

        }
    }
}
