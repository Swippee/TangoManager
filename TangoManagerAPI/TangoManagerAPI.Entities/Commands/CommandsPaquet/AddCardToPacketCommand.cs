using System.Threading.Tasks;
using TangoManagerAPI.Entities.Models;
using TangoManagerAPI.Entities.Ports.Routers;

namespace TangoManagerAPI.Entities.Commands.CommandsPaquet
{
    public  class AddCardToPacketCommand : ACommand<PacketAggregate, AddCardToPacketCommand>
    {
        public string PacketName { get; }
        public string Question { get; }
        public string Answer { get; }
        public decimal Score { get; }

        public AddCardToPacketCommand(string packetName, string question, string answer, decimal score)
        {
            PacketName = packetName;
            Question = question;
            Answer = answer;
            Score = score;
        }

        public override async Task<PacketAggregate> ExecuteAsync(ICommandRouter commandRouter)
        {
            return await commandRouter.RouteAwaitForResultAsync(this);
        }
    }
}
