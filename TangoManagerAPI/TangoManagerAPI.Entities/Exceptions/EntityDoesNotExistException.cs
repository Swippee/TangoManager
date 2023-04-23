using TangoManagerAPI.Entities.Ports.Exceptions;

namespace TangoManagerAPI.Entities.Exceptions
{
    public class EntityDoesNotExistException : TangoManagerException
    {
        public EntityDoesNotExistException(string message)
            : base(message)
        {
        }
    }
}
