using System;
using System.Threading;
using System.Threading.Tasks;
using TangoManagerAPI.Entities.Queries;

namespace TangoManagerAPI.Entities.Ports.Handler
{
    public interface IQueryHandler<T, S> : IQueryHandler where S : AQuery
    {
        public Type SupportedQueryType => typeof(S);

        Task<T> HandleAsync(S query, CancellationToken token = default);
    }

    public interface IQueryHandler
    {
        public Type GetSupportedQueryType<S>() where S : AQuery => typeof(S);

    }

    public interface IQueryHandler<S> : IQueryHandler where S : AQuery
    {
        public Type SupportedQueryType => typeof(S);

        Task HandleAsync(S query, CancellationToken token = default);
    }

}
