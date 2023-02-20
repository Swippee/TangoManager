using System;
using System.Collections.Generic;
using System.Text;
using TangoManagerAPI.Infrastructures.Routers;

namespace TangoManagerAPI.Entities.Events
{

    [Serializable]
    public abstract class AEvent<TData> : AEvent
    {
        public TData Data { get; }

        protected AEvent(TData data)
        {
            Data = data;
        }
    }

    [Serializable]
    public abstract class AEvent
    {
        public abstract void Dispatch(IEventRouter eventRouter);
    }
}
