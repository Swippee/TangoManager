using TangoManagerAPI.Entities.Exceptions;

namespace TangoManagerAPI.Entities.Models
{
    public  class QuizFinishedState : IQuizState
    {
        public static string StateName => "Finished";

        public void Answer(string answer)
        {
            throw new QuizAlreadyFinishedException("QuizAggregate has already been finished!");
        }

        public override string ToString()
        {
            return StateName;
        }
    }
}
