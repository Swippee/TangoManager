using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TangoManagerAPI.Entities.Events;

namespace TangoManagerAPI.Entities.Ports.Router

{
    public interface IEventRouter
    {
        public Task RouteAsync<TEvent>(TEvent @event) where TEvent : AEvent;
    }
}
