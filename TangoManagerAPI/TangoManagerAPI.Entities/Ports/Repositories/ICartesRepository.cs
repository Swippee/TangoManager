using System.Collections.Generic;
using System.Threading.Tasks;
using TangoManagerAPI.Entities.Models;

namespace TangoManagerAPI.Entities.Ports.Repositories;

public interface ICartesRepository
{
    Task AddCarteAsync(CarteEntity Carte);
    Task<IEnumerable<CarteEntity>> GetCartesAsync();
    Task<IEnumerable<CarteEntity>> GetCartesByPaquetNameAsync(string paquetName);
}