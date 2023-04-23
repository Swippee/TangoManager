using System;

namespace TangoManagerAPI.Entities.Ports.Exceptions
{
    public abstract class TangoManagerException : Exception
    {
        protected TangoManagerException(string message)
            : base(message)
        {

        }
    }
}
