using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TangoManagerAPI.Entities.Ports.Handler;
using TangoManagerAPI.Entities.Ports.Router;
using TangoManagerAPI.Entities.Queries;

namespace TangoManagerAPI.Infrastructures.Routers
{
    public class QueryRouter : IQueryRouter
    {
        private readonly IDictionary<Type, IQueryHandler> _queryHandlers;

        public QueryRouter()
        {
            _queryHandlers = new Dictionary<Type, IQueryHandler>();
        }

        public async Task<TResult> RouteAsync<TResult, TQuery>(AQuery<TResult, TQuery> query) where TQuery : AQuery<TResult, TQuery>
        {
            if (_queryHandlers.TryGetValue(query.QueryType, out var queryHandler))
                return await ((IQueryHandler<TResult, TQuery>)queryHandler).HandleAsync((TQuery)query);
            throw new KeyNotFoundException();
        }

        public void AddQueryHandler<TQuery>(IQueryHandler handler) where TQuery : AQuery
        {
            _queryHandlers.Add(handler.GetSupportedQueryType<TQuery>(), handler);
        }
    }
}
