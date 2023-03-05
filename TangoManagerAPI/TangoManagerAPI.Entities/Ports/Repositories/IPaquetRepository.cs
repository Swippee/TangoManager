using System.Collections.Generic;
using System.Threading.Tasks;
using TangoManagerAPI.Entities.Models;

namespace TangoManagerAPI.Entities.Ports.Repositories;

public interface IPaquetRepository
{
    Task AddPaquetAsync(PaquetEntity paquet);
    Task RemovePaquetAsync(string name);
    Task<IEnumerable<PaquetEntity>> GetPaquetsAsync();
    Task<PaquetEntity> GetPaquetByNameAsync(string name);
}