using TangoManagerAPI.Entities.Ports.Exceptions;

namespace TangoManagerAPI.Entities.Exceptions
{
    public class EmptyPaquetException : TangoManagerException
    {
        public EmptyPaquetException(string message)
            : base(message)
        {
        }
    }
}
