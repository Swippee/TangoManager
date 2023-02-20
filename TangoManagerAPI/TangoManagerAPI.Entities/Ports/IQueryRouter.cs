using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TangoManagerAPI.Entities.Queries;

namespace TangoManagerAPI.Infrastructures.Routers
{
    public interface IQueryRouter
    {
         Task<TResult> RouteAsync<TResult, TQuery>(AQuery<TResult, TQuery> query) where TQuery : AQuery<TResult, TQuery>;
    }
}
