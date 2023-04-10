using Microsoft.AspNetCore.Mvc;
using System.Net;
using TangoManagerAPI.DTO;
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
        public async Task<ActionResult> CreateAsync([FromBody] CreateQuizRequest createQuizRequest)
        {
            var cmd = new CreateQuizCommand(createQuizRequest.PacketName);
            var quizAggregate = await cmd.ExecuteAsync(_commandRouter);

            return StatusCode((int)HttpStatusCode.Created, new
            {
                Quiz = quizAggregate.RootEntity,
                Packet = quizAggregate.PacketEntity
            });
        }

        [HttpPost]
        [Route("Answer")]
        public async Task<ActionResult> AnswerAsync(AnswerQuizRequest answerQuizRequest)
        {
            var cmd = new AnswerQuizCommand(answerQuizRequest.QuizId, answerQuizRequest.Answer);
            var quizAggregate = await cmd.ExecuteAsync(_commandRouter);

            return StatusCode((int)HttpStatusCode.OK, new
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
