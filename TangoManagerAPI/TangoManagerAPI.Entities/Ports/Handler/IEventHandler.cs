using System;
using TangoManagerAPI.Entities.Events;

namespace TangoManagerAPI.Entities.Ports.Handler
{
    public interface IEventHandler<in TEvent> : IEventHandler where TEvent : AEvent
    {
        public Type SupportedEventType => typeof(TEvent);

        void Handle(TEvent @event);
    }

    public interface IEventHandler
    {
        public Type GetSupportedEventType<TEvent>() where TEvent : AEvent => typeof(TEvent);
    }
}
