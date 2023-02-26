using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TangoManagerAPI.Entities.Ports.Router;

namespace TangoManagerAPI.Entities.Queries
{
    public abstract class AQuery<TResult, TQuery> : AQuery
    {
        public Type QueryType => typeof(TQuery);

        public abstract Task<TResult> QueryAsync(IQueryRouter queryRouter);
    }
    public abstract class AQuery<TQuery> : AQuery
    {
        public Type QueryType => typeof(TQuery);

        public abstract Task QueryAsync(IQueryRouter queryRouter);
    }
    public abstract class AQuery
    {

    }
}
