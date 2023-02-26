using System.Threading.Tasks;
using TangoManagerAPI.Entities.Commands.CommandsPaquet;
using TangoManagerAPI.Entities.Exceptions;
using TangoManagerAPI.Entities.Ports.Handler;
using TangoManagerAPI.Entities.Ports.Repository;
using TangoManagerAPI.Models;

namespace TangoManagerAPI.Infrastructures.Handlers
{
    public class CommandHandler :
        ICommandHandler<PaquetEntity, CreatePaquetAsyncCommand>,
        ICommandHandler<PaquetEntity, UpdatePaquetAsyncCommand>
    {
        private readonly IPaquetRepository _paquetRepository;
        

        public CommandHandler(
            IPaquetRepository paquetRepository
            )
        {
            _paquetRepository = paquetRepository;
            
        }

        public async Task<PaquetEntity> HandleAsync(CreatePaquetAsyncCommand command)
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

        public async Task<PaquetEntity> HandleAsync(UpdatePaquetAsyncCommand command)
        {
            var paquetEntity = await _paquetRepository.GetPaquetByNameAsync(command.Name);

            if (paquetEntity == null)
            {
                throw new EntityDoNotExistException($"Move entity with name {paquetEntity.Nom} doesn exists, cannot add move with duplicate name.");
            }

            paquetEntity = new PaquetEntity
            {
                Nom = command.Name,
                Description = command.Description,

            };

            await _paquetRepository.UpdatePaquetAsync(paquetEntity);
            return paquetEntity;
        }
    }
}
