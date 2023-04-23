using TangoManagerAPI.Entities.Ports.Exceptions;

namespace TangoManagerAPI.Entities.Exceptions
{
    public class CardNotFoundInPacketException : TangoManagerException
    {
        public CardNotFoundInPacketException(string message)
            : base(message)
        {
        }
    }
}
