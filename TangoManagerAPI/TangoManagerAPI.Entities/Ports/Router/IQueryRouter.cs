using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TangoManagerAPI.Entities.Queries;

namespace TangoManagerAPI.Entities.Ports.Router
{
    public interface IQueryRouter
    {
        public Task<TResult> RouteAsync<TResult, TQuery>(AQuery<TResult, TQuery> query) where TQuery : AQuery<TResult, TQuery>;
        public Task RouteAsync<TQuery>(AQuery<TQuery> query) where TQuery : AQuery<TQuery>;
    }
}
