using System.Threading.Tasks;
using TangoManagerAPI.Entities.Models;
using TangoManagerAPI.Entities.Ports.Routers;

namespace TangoManagerAPI.Entities.Commands.CommandsCard
{
    public sealed class CreateCardCommand : ACommand<CarteEntity, CreateCardCommand>
    {
        public string PacketName { get; }
        public string Question { get; }
        public string Answer { get; }


        public CreateCardCommand(string packetName, string question, string answer)
        {
            PacketName = packetName;
            Question = question;
            Answer = answer;

          
        }

        public override async Task<CarteEntity> ExecuteAsync(ICommandRouter commandRouter)
        {
            return await commandRouter.RouteAwaitForResultAsync(this);
        }
    }
}