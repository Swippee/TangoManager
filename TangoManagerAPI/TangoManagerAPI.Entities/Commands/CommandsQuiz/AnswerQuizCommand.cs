using System.Threading.Tasks;
using TangoManagerAPI.Entities.Models;
using TangoManagerAPI.Entities.Ports.Routers;

namespace TangoManagerAPI.Entities.Commands.CommandsQuiz
{
    public sealed class AnswerQuizCommand : ACommand<QuizAggregate, AnswerQuizCommand>
    {
        public int QuizId { get; }
        public string Answer { get; }

        public AnswerQuizCommand(int quizId, string answer)
        {
            QuizId = quizId;
            Answer = answer;
        }

        public override async Task<QuizAggregate> ExecuteAsync(ICommandRouter commandRouter)
        {
            return await commandRouter.RouteAwaitForResultAsync(this);
        }
    }
}
