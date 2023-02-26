using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using TangoManagerAPI.DTO;
using TangoManagerAPI.Entities.Commands;
using TangoManagerAPI.Entities.Commands.CommandsPaquet;
using TangoManagerAPI.Entities.Exceptions;
using TangoManagerAPI.Entities.Ports.Exceptions;
using TangoManagerAPI.Entities.Ports.Repository;
using TangoManagerAPI.Entities.Ports.Router;
using TangoManagerAPI.Entities.Queries;
using TangoManagerAPI.Infrastructures.Routers;
using TangoManagerAPI.Models;

namespace TangoManagerAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PaquetController : ControllerBase
    {

        private readonly IQueryRouter _queryRouter;
        private readonly ICommandRouter _commandRouter;
        private readonly IPaquetRepository _paquetRepository;

        public PaquetController(IQueryRouter queryRouter, ICommandRouter commandRouter, IPaquetRepository paquetRepository)
        {
            _queryRouter = queryRouter;
            _commandRouter = commandRouter;
            _paquetRepository = paquetRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<string>>> GetAllPaquet()
        {
            try
            {
                var res = await new GetPaquetsQuery().QueryAsync(_queryRouter);
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

        [HttpGet("Test")]
        public List<PaquetEntity> TestGetAllPaquet()
        {
            var records = _paquetRepository.TestList().ToList();
            return records;
        }


        [HttpGet("Detail/{name}")]
        public async Task<ActionResult<List<string>>> GetPaquetByName(string name)
        {
            try
            {
                var res = await new GetPaquetByNameQuery(name).QueryAsync(_queryRouter);
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
        [HttpPost("Create")]
        public async Task<ActionResult> NewPaquet([FromQuery] CreatePaquetDTO dto)
        {
            try
            {
                var res = await new CreatePaquetAsyncCommand(dto.Name, dto.Description).ExecuteAsync(_commandRouter);
                return StatusCode(201, res);
            }
            catch (Exception e)
            {
                if (e is EntityAlreadyExistsException entityAlreadyExistsException)
                    return StatusCode(409, entityAlreadyExistsException.Message);
                throw;
            }
        }

        [HttpPost("Update")]
        public async Task<ActionResult> UpdatePaquet([FromQuery] CreatePaquetDTO dto)
        {
            try
            {
                var res = await new UpdatePaquetAsyncCommand(dto.Name,dto.Description).ExecuteAsync(_commandRouter);
                return StatusCode(201, res);
            }
            catch (Exception e)
            {
                if (e is EntityDoNotExistException entityDoNotExistException)
                    return StatusCode(409, entityDoNotExistException.Message);
                throw;
            }
        }

        [HttpDelete("Delete/{name}")]
        public async Task<StatusCodeResult> DeletePaquet(string name)
        {
            try
            {
                await new DeletePaquetByNameQuery(name).QueryAsync(_queryRouter);
            
            }
            catch (Exception e)
            {
                    return StatusCode(409);
                throw;
            }
            return StatusCode(201);
        }
    }
}
