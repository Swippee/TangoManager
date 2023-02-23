using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TangoManagerAPI.Models;

namespace TangoManagerAPI.Entities.Ports.Repository
{
    public interface IPaquetRepository
    {
        Task AddPaquetAsync(PaquetEntity paquet);
        Task RemovePaquetAsync(string name);

        Task<IEnumerable<PaquetEntity>> GetPaquetsAsync();
        Task<PaquetEntity> GetPaquetByName(string name);
    }
}
