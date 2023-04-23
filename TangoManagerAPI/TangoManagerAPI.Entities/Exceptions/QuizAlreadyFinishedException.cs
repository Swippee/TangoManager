using TangoManagerAPI.Entities.Ports.Exceptions;

namespace TangoManagerAPI.Entities.Exceptions
{
    public class QuizAlreadyFinishedException : TangoManagerException
    {
        public QuizAlreadyFinishedException(string message)
            : base(message)
        {
        }
    }
}
