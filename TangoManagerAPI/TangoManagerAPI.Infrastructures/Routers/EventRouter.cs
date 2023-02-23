using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TangoManagerAPI.Entities.Events;
using TangoManagerAPI.Entities.Ports.Handler;
using TangoManagerAPI.Entities.Ports.Router;

namespace TangoManagerAPI.Infrastructures.Routers
{
    public sealed class EventRouter : IEventRouter
    {
        private readonly IDictionary<Type, HashSet<IEventHandler>> _eventHandlers;

        public EventRouter()
        {
            _eventHandlers = new Dictionary<Type, HashSet<IEventHandler>>();
        }

        public void AddEventHandler<TEvent>(IEventHandler handler) where TEvent : AEvent
        {
            if (_eventHandlers.TryGetValue(handler.GetSupportedEventType<TEvent>(), out var handlers))
            {
              handlers.Add(handler);
            }
            else
            {
                handlers = new HashSet<IEventHandler> { handler };
                _eventHandlers[handler.GetSupportedEventType<TEvent>()] = handlers;
            }
        }

        public Task RouteAsync<TEvent>(TEvent @event) where TEvent : AEvent
        {
            if (!_eventHandlers.TryGetValue(@event.GetType(), out var handlers)) 
                return Task.CompletedTask;
          
            foreach (var eventHandler in handlers)
            {
                ((IEventHandler<TEvent>)eventHandler).Handle(@event);
            }

            return Task.CompletedTask;
        }
    }
}
