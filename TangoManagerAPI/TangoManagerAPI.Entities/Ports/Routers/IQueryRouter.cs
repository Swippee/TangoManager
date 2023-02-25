using System.Threading.Tasks;
using TangoManagerAPI.Entities.Queries;

namespace TangoManagerAPI.Entities.Ports.Routers;

public interface IQueryRouter
{
    public Task<TResult> RouteAsync<TResult, TQuery>(AQuery<TResult, TQuery> query) where TQuery : AQuery<TResult, TQuery>;
}