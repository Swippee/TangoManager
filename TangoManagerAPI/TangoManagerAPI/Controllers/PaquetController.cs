using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using TangoManagerAPI.Entities.Ports.Repository;
using TangoManagerAPI.Models;

namespace TangoManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaquetController : ControllerBase
    {
        private readonly IReadRepository _paquetReadRepository;
        private readonly IWriteRepository _paquetWriteRepository;
        public PaquetController(IReadRepository readRepository, IWriteRepository writeRepository)
        {
            _paquetReadRepository = readRepository;
            _paquetWriteRepository = writeRepository;

        }

        [HttpGet]
        public async Task<ActionResult<List<PaquetEntity>>> GetAllPaquet()
        {
            var paquets = _paquetReadRepository.GetPaquetsAsync().Result.OrderBy(x => x.Nom).ToList();
            return Ok(paquets);
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<PaquetEntity>> GetPaquetByName(string name)
        {
            var paquet = _paquetReadRepository.GetPaquetByName(name).Result;
            return Ok(paquet);
        }

        [HttpPost]
        public IActionResult InsertPaquet(PaquetEntity entity)
        {
            try
            {
                _paquetWriteRepository.AddPaquet(entity);
            }
            catch
            {

                return Ok(new { Success = false });
            }
            return Ok(new { Success = true });
        }

        [HttpDelete]
        public IActionResult DeletePaquet(string name)
        {
            try
            {
                _paquetWriteRepository.RemovePaquet(name);
            }
            catch
            {

                return Ok(new { Success = false });
            }
            return Ok(new { Success = true });
        }
    }
}
