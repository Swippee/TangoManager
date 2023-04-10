using Microsoft.AspNetCore.Mvc;
using TangoManagerAPI.Entities.Commands.CommandsPaquet;
using TangoManagerAPI.Entities.Models;
using TangoManagerAPI.Entities.Ports.Routers;
using TangoManagerAPI.Entities.Queries;

namespace TangoManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaquetsController : ControllerBase
    {
        private readonly IQueryRouter _queryRouter;
        private readonly ICommandRouter _commandRouter;

        public PaquetsController(IQueryRouter queryRouter, ICommandRouter commandRouter)
        {
            _queryRouter = queryRouter;
            _commandRouter = commandRouter;
        }

        [HttpGet]
        [Route("")]
        [Route("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            var res = await new GetAllPaquetsQuery().QueryAsync(_queryRouter);
            List<PaquetEntity> result = new List<PaquetEntity>();
            res.ToList().ForEach(i => result.Add(i.RootEntity));

            return StatusCode(200, res);
        }

        [HttpPost]
        [Route("")]
        [Route("Index")]
        public async Task<ActionResult> CreateAsync([FromBody] CreatePaquetCommand createPacketCommand)
        {
            var packetAggregate = await createPacketCommand.ExecuteAsync(_commandRouter);

            return StatusCode(200, packetAggregate.RootEntity);
        }

        [HttpDelete]
        [Route("{name}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] string name)
        {
            DeletePaquetCommand deletePacketCommand = new(name);

            var packetAggregate = await deletePacketCommand.ExecuteAsync(_commandRouter);

            return StatusCode(200, packetAggregate.RootEntity);
        }

        [HttpPut]
        public async Task<ActionResult> AddCardAsync([FromBody] AddCardToPacketCommand addCardToPacketCommand)
        {
            var packetAggregate = await addCardToPacketCommand.ExecuteAsync(_commandRouter);

            return StatusCode(200, packetAggregate.RootEntity);
        }
    }
}
