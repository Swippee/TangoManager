using System;
using System.Threading.Tasks;
using TangoManagerAPI.Entities.Events;

namespace TangoManagerAPI.Entities.Ports.Handlers
{
    public interface IEventHandler<in TEvent> : IEventHandler where TEvent : AEvent
    {
        public Type SupportedEventType => typeof(TEvent);

        Task HandleAsync(TEvent @event);
    }

    public interface IEventHandler
    {
        public Type GetSupportedEventType<TEvent>() where TEvent : AEvent => typeof(TEvent);
    }
}
