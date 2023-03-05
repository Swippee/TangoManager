using Microsoft.AspNetCore.Mvc;
using TangoManagerAPI.Entities.Commands.CommandsQuiz;
using TangoManagerAPI.Entities.Ports.Routers;

namespace TangoManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
       
        private readonly ICommandRouter _commandRouter;

        public QuizController(ICommandRouter commandRouter)
        {
            _commandRouter = commandRouter;
        }

        [HttpPost]
        [Route("")]
        [Route("Index")]
        public async Task<ActionResult> CreateAsync([FromBody] CreateQuizCommand createQuizCommand)
        {
            var quizAggregate = await createQuizCommand.ExecuteAsync(_commandRouter);

            return StatusCode(200, new
            {
                Quiz = quizAggregate.RootEntity,
                Packet = quizAggregate.PacketEntity
            });
        }

        [HttpPost]
        [Route("Answer")]
        public async Task<ActionResult> AnswerAsync([FromBody] AnswerQuizCommand answerQuizCommand)
        {
            var quizAggregate = await answerQuizCommand.ExecuteAsync(_commandRouter);

            return StatusCode(200, new
            {
                Quiz = quizAggregate.RootEntity,
                Packet = quizAggregate.PacketEntity,
                CurrentCard = quizAggregate.CurrentCard,
                AnsweredCards = quizAggregate.AnsweredCards,
                CorrectlyAnsweredCards = quizAggregate.CorrectlyAnsweredCards,
                IncorrectlyAnsweredCards = quizAggregate.IncorrectlyAnsweredCards
            });
        }
    }
}
