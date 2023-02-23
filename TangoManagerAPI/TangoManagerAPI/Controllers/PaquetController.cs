using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using TangoManagerAPI.Entities.Ports.Exceptions;
using TangoManagerAPI.Entities.Ports.Repository;
using TangoManagerAPI.Entities.Ports.Router;
using TangoManagerAPI.Entities.Queries;
using TangoManagerAPI.Models;

namespace TangoManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaquetController : ControllerBase
    {
        //private readonly IReadRepository _paquetReadRepository;
        //private readonly IWriteRepository _paquetWriteRepository;
        //public PaquetController(IReadRepository readRepository, IWriteRepository writeRepository)
        //{
        //    _paquetReadRepository = readRepository;
        //    _paquetWriteRepository = writeRepository;

        //}

        //[HttpGet]
        //public async Task<ActionResult<List<PaquetEntity>>> GetAllPaquet()
        //{
        //    var paquets = await _paquetReadRepository.GetPaquetsAsync();
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

        public PaquetController(IQueryRouter queryRouter)
        {
            _queryRouter = queryRouter;
        }

        [HttpGet]
        public async Task<ActionResult<List<string>>> GetAllPaquet()
        {
            try
            {
                var res = await new AQueryPaquet().QueryAsync(_queryRouter);
                return StatusCode(200, res);
            }
            catch (Exception e)
            {
                if (e is TangoManagerException tangoManagerException)
                {
                    Console.WriteLine(tangoManagerException.Message);
                    return StatusCode(204);
                }
                throw;
            }
        }



    }
}
