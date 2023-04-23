using Microsoft.AspNetCore.Mvc;
using System.Net;
using TangoManagerAPI.DTO;
using TangoManagerAPI.Entities.Commands.CommandsLock;
using TangoManagerAPI.Entities.Commands.CommandsQuiz;
using TangoManagerAPI.Entities.Models;
using TangoManagerAPI.Entities.Ports.Repositories;
using TangoManagerAPI.Entities.Ports.Routers;
using TangoManagerAPI.Entities.Queries;
using TangoManagerAPI.Infrastructures.Routers;

namespace TangoManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LockerController : ControllerBase
    {
       
        private readonly ICommandRouter _commandRouter;
        public LockerController(ICommandRouter commandRouter)
        {
            _commandRouter = commandRouter;
        }


        [HttpPut]
        [Route("{packetName}")]
        public async Task<ActionResult> InsertLock([FromRoute] string packetName)
        {
            var cmd = new AddLockToPacketCommand(packetName);
            var lockResult= await cmd.ExecuteAsync(_commandRouter);

            return StatusCode((int)HttpStatusCode.OK, new { lockResult.token });
        }

    }
}
