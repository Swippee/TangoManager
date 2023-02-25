using System.Threading.Tasks;
using TangoManagerAPI.Entities.Commands.CommandsPaquet;
using TangoManagerAPI.Entities.Exceptions;
using TangoManagerAPI.Entities.Models;
using TangoManagerAPI.Entities.Ports.Handler;
using TangoManagerAPI.Entities.Ports.Handlers;
using TangoManagerAPI.Entities.Ports.Repositories;

namespace TangoManagerAPI.Infrastructures.Handlers
{
    public class CommandHandler :
        ICommandHandler<PaquetEntity, CreateNewPaquetCommand>
    {
        private readonly IPaquetRepository _paquetRepository;
        

        public CommandHandler(
            IPaquetRepository paquetRepository
            )
        {
            _paquetRepository = paquetRepository;
            
        }

        public async Task<PaquetEntity> HandleAsync(CreateNewPaquetCommand command)
        {
           var paquetEntity = await _paquetRepository.GetPaquetByNameAsync(command.Name);

            if (paquetEntity != null) {
                throw new EntityAlreadyExistsException($"Move entity with name {paquetEntity.Nom} already exists, cannot add move with duplicate name.");
            }

            paquetEntity = new PaquetEntity
            {
                Nom=command.Name,
                Description=command.Description,
            };

            await _paquetRepository.AddPaquetAsync(paquetEntity);
            return paquetEntity;
        }
    }
}
