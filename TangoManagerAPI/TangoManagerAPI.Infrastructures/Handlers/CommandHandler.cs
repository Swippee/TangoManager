using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangoManagerAPI.Entities.Commands;
using TangoManagerAPI.Entities.Exceptions;
using TangoManagerAPI.Entities.Ports.Handler;
using TangoManagerAPI.Entities.Ports.Repository;
using TangoManagerAPI.Entities.Ports.Router;
using TangoManagerAPI.Models;

namespace TangoManagerAPI.Infrastructures.Handlers
{
    public class CommandHandler :
        ICommandHandler<PaquetEntity, CreateNewPaquetAsyncCommand>
    {
        private readonly IPaquetRepository _paquetRepository;
        

        public CommandHandler(
            IPaquetRepository paquetRepository
            )
        {
            _paquetRepository = paquetRepository;
            
        }

        public async Task<PaquetEntity> HandleAsync(CreateNewPaquetAsyncCommand command)
        {
           var paquetEntity = await _paquetRepository.GetPaquetByName(command.Name);

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
