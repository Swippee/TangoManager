using Microsoft.AspNetCore.Mvc;
using TangoManagerAPI.Entities.Commands.CommandsPaquet;
using TangoManagerAPI.Entities.Ports.Exceptions;
using TangoManagerAPI.Entities.Ports.Routers;
using TangoManagerAPI.Entities.Queries;

namespace TangoManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaquetsController : ControllerBase
    {
        //private readonly IReadRepository _paquetReadRepository;
        //private readonly IWriteRepository _paquetWriteRepository;
        //public PaquetsController(IReadRepository readRepository, IWriteRepository writeRepository)
        //{
        //    _paquetReadRepository = readRepository;
        //    _paquetWriteRepository = writeRepository;

        //}

        //[HttpGet]
        //public async Task<ActionResult<List<PaquetEntity>>> GetAllPaquet()
        //{
        //    var paquets = await _paquetReadRepository.GetPacketsAsync();
        //    return Ok(paquets.OrderBy(x => x.Nom).ToList());
        //}

        //[HttpGet("{name}")]
        //public async Task<ActionResult<PaquetEntity>> GetPaquetByName(string name)
        //{
        //    var paquet = await _paquetReadRepository.GetPaquetByName(name);
        //    return Ok(paquet);
        //}

        //[HttpPost]
        //public IActionResult InsertPaquet(PaquetEntity entity)
        //{
        //    try
        //    {
        //        _paquetWriteRepository.AddPaquetAsync(entity);
        //    }
        //    catch
        //    {

        //        return Ok(new { Success = false });
        //    }
        //    return Ok(new { Success = true });
        //}

        //[HttpDelete]
        //public IActionResult DeletePaquet(string name)
        //{
        //    try
        //    {
        //        _paquetWriteRepository.RemovePaquetAsync(name);
        //    }
        //    catch
        //    {

        //        return Ok(new { Success = false });
        //    }
        //    return Ok(new { Success = true });
        //}
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
            try
            {
                var res = await new GetAllPaquetsQuery().QueryAsync(_queryRouter);
                return StatusCode(200, res);
            }
            catch (Exception e)
            {
                if (e is TangoManagerException tangoManagerException)
                {
                    Console.WriteLine(tangoManagerException.Message);
                }
                throw;
            }
        }

        [HttpPost]
        [Route("")]
        [Route("Index")]
        public async Task<ActionResult> CreateAsync([FromBody] CreatePaquetCommand createPacketCommand)
        {
            var packetAggregate = await createPacketCommand.ExecuteAsync(_commandRouter);

            return StatusCode(200, packetAggregate.RootEntity);
        }

        [HttpPut]
        [Route("{packetName}")]
        public async Task<ActionResult> AddCardAsync([FromRoute] string packetName, [FromBody] AddCardToPacketCommand addCardToPacketCommand)
        {
            var packetAggregate = await addCardToPacketCommand.ExecuteAsync(_commandRouter);

            return StatusCode(200, packetAggregate.RootEntity);
        }
    }
}
