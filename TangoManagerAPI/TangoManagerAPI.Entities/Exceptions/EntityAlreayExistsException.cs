using TangoManagerAPI.Entities.Ports.Exceptions;

namespace TangoManagerAPI.Entities.Exceptions
{
    public class EntityAlreadyExistsException : TangoManagerException
    {
        public EntityAlreadyExistsException(string message)
            : base(message)
        {
        }
    }
}
