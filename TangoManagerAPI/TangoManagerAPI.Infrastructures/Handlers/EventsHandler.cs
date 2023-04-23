using TangoManagerAPI.Entities.Events;
using TangoManagerAPI.Entities.Events.PacketLockEntityEvents;
using TangoManagerAPI.Entities.Events.QuizAggregateEvents;
using TangoManagerAPI.Entities.Ports.Handlers;

namespace TangoManagerAPI.Infrastructures.Handlers
{
    public class EventsHandler : 
        IEventHandler<QuizAnsweredEvent>, 
        IEventHandler<QuizCardEntityAddedEvent>,
        IEventHandler<PacketUpdatedEvent>,
        IEventHandler<CardUpdatedEvent>,
        IEventHandler<PacketLockAccessedEvent>
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

        public async void Handle(PacketLockAccessedEvent @event)
        {
            //TODO
            //implement repository
            //make update @event.Data.LastAccessedDateTime
        }
    }
}
