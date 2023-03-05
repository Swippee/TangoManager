using TangoManagerAPI.Entities.Ports.Routers;

namespace TangoManagerAPI.Entities.Events
{
    public abstract class AEvent<TData> : AEvent
    {
        public TData Data { get; }

        protected AEvent(TData data)
        {
            Data = data;
        }
    }

    public abstract class AEvent
    {
        public abstract void Dispatch(IEventRouter eventRouter);
    }


}
