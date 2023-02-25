using System.Threading.Tasks;
using TangoManagerAPI.Entities.Events;

namespace TangoManagerAPI.Entities.Ports.Routers;

public interface IEventRouter
{
    public Task RouteAsync<TEvent>(TEvent @event) where TEvent : AEvent;
}