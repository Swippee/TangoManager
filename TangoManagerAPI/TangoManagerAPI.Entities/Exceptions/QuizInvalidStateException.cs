using TangoManagerAPI.Entities.Ports.Exceptions;

namespace TangoManagerAPI.Entities.Exceptions
{
    public class QuizInvalidStateException : TangoManagerException
    {
        public QuizInvalidStateException(string message)
            : base(message)
        {
        }
    }
}
