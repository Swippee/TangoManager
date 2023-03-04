using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using TangoManagerAPI.Entities.Commands.CommandsQuiz;
using TangoManagerAPI.Entities.Ports.Exceptions;
using TangoManagerAPI.Entities.Ports.Routers;
using TangoManagerAPI.Entities.Queries;

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
        public async Task<ActionResult> CreateAsync([FromBody] string packetName)
        {
            var quizAggregate = await new CreateQuizCommand(packetName).ExecuteAsync(_commandRouter);

            return StatusCode(200, new
            {
                Quiz = quizAggregate.RootEntity,
                Packet = quizAggregate.PacketEntity,
                PacketCards = quizAggregate.PacketCards
            });
        }

        [HttpPost]
        [Route("Answer")]
        public async Task<ActionResult> AnswerAsync([FromBody] int quizId, [FromBody] string answer)
        {
            var quizAggregate = await new AnswerQuizCommand(quizId, answer).ExecuteAsync(_commandRouter);

            return StatusCode(200, new
            {
                Quiz = quizAggregate.RootEntity,
                Packet = quizAggregate.PacketEntity,
                PacketCards = quizAggregate.PacketCards,
                CurrentCard = quizAggregate.CurrentCard,
                AnsweredCards = quizAggregate.AnsweredCards,
                CorrectlyAnsweredCards = quizAggregate.CorrectlyAnsweredCards,
                IncorrectlyAnsweredCards = quizAggregate.IncorrectlyAnsweredCards
            });
        }
    }
}
