using System.Threading.Tasks;
using TangoManagerAPI.Entities.Models;
using TangoManagerAPI.Entities.Ports.Routers;

namespace TangoManagerAPI.Entities.Commands.CommandsQuiz
{
    public sealed class CreateQuizCommand : ACommand<QuizAggregate, CreateQuizCommand>
    {
        public CreateQuizCommand(string packetName)
        {
            PacketName = packetName;
        }

        public string PacketName { get; }

        
        public override async Task<QuizAggregate> ExecuteAsync(ICommandRouter commandRouter)
        {
            return await commandRouter.RouteAwaitForResultAsync(this);
        }
    }
}
