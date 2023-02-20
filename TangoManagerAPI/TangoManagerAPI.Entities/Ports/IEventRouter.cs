using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TangoManagerAPI.Entities.Events;

namespace TangoManagerAPI.Infrastructures.Routers
{
    public interface IEventRouter
    {
         Task RouteAsync<TEvent>(TEvent @event) where TEvent : AEvent;
    }
}
