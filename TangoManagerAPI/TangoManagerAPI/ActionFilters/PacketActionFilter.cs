using Microsoft.AspNetCore.Mvc.Filters;
using TangoManagerAPI.Application.Queries.CommandsAuth;
using TangoManagerAPI.Entities.Exceptions;
using TangoManagerAPI.Entities.Ports.Routers;

namespace TangoManagerAPI.ActionFilters
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class PacketActionFilter : Attribute, IAsyncActionFilter
    {
        private IQueryRouter? _queryRouter;
        private IEventRouter? _eventRouter;

        private const string HeaderName = "PacketToken";
        private readonly string _packetParameterName;

        public PacketActionFilter(string packetParameterName)
        {
            _packetParameterName = packetParameterName;
        }

        
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _queryRouter = context.HttpContext.RequestServices.GetRequiredService<IQueryRouter>();
            _eventRouter = context.HttpContext.RequestServices.GetRequiredService<IEventRouter>();

            if (!context.RouteData.Values.TryGetValue(_packetParameterName, out var packetName))
            {
                await next();
                return;
            }

            var packetLockEntity = await new GetPacketLockQuery(packetName!.ToString()!).QueryAsync(_queryRouter);

            if (packetLockEntity == null)
            {
                await next();
                return;
            }

            if (!context.HttpContext.Request.Headers.TryGetValue(HeaderName, out var headerValues))
            {
                throw new InvalidPacketTokenHeaderException($"No header with name {HeaderName} was provided!");
            }

            if (headerValues != packetLockEntity.LockToken)
            {
                throw new InvalidPacketTokenException($"Provided packet lock token for packet {packetName} is not valid!");
            }

            var lastAccessedEvent = packetLockEntity.UpdateLastAccessedDateTime();
            lastAccessedEvent.Dispatch(_eventRouter);

            await next();
        }

    }
}
