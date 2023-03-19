using TangoManagerAPI.Entities.Ports.Exceptions;

namespace TangoManagerAPI.Entities.Exceptions
{
    public class CardAlreadyExistsInPacketException : TangoManagerException
    {
        public CardAlreadyExistsInPacketException(string message)
            : base(message)
        {
        }
    }
}
