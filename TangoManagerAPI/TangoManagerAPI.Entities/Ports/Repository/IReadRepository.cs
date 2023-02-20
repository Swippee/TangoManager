using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TangoManagerAPI.Models;

namespace TangoManagerAPI.Entities.Ports.Repository
{
    public interface IReadRepository
    {
        Task<IEnumerable<PaquetEntity>> GetPaquetsAsync();
        Task<PaquetEntity> GetPaquetByName(string name);

    }
}
