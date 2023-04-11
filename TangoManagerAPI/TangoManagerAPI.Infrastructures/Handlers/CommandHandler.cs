using System.Linq;
using System.Threading.Tasks;
using TangoManagerAPI.Entities.Commands.CommandsPaquet;
using TangoManagerAPI.Entities.Commands.CommandsQuiz;
using TangoManagerAPI.Entities.Exceptions;
using TangoManagerAPI.Entities.Models;
using TangoManagerAPI.Entities.Ports.Handlers;
using TangoManagerAPI.Entities.Ports.Repositories;
using TangoManagerAPI.Entities.Ports.Routers;

namespace TangoManagerAPI.Infrastructures.Handlers
{
    public class CommandHandler :
        ICommandHandler<PacketAggregate, CreatePaquetCommand>,
        ICommandHandler<Task, DeletePaquetCommand>,
        ICommandHandler<QuizAggregate, AnswerQuizCommand>,
        ICommandHandler<QuizAggregate, CreateQuizCommand>,
        ICommandHandler<PacketAggregate, AddCardToPacketCommand>
    {
        private readonly IPaquetRepository _paquetRepository;
        private readonly IEventRouter _eventRouter;
        private readonly IQuizRepository _quizRepository;


        public CommandHandler(IPaquetRepository paquetRepository, IEventRouter eventRouter, IQuizRepository quizRepository)
        {
            _paquetRepository = paquetRepository;
            _eventRouter = eventRouter;
            _quizRepository = quizRepository;
        }

        public async Task<PacketAggregate> HandleAsync(CreatePaquetCommand command)
        {
            var packetAggregate = await _paquetRepository.GetPacketByNameAsync(command.Name);

            if (packetAggregate != null)
                throw new EntityAlreadyExistsException($"Packet with name {packetAggregate.RootEntity.Name} already exists in base");

            var packetEntity = new PaquetEntity(command.Name, command.Description);

            packetAggregate = new PacketAggregate(packetEntity);

            await _paquetRepository.SavePacketAsync(packetAggregate);

            return packetAggregate;
        }

        public async Task<QuizAggregate> HandleAsync(AnswerQuizCommand command)
        {
            var quizAggregate = await _quizRepository.GetQuizByIdAsync(command.QuizId);

            var events = quizAggregate.Answer(command.Answer);

            await _quizRepository.SaveQuizAsync(quizAggregate);

            foreach (var @event in events)
            {
                @event.Dispatch(_eventRouter);
            }

            return quizAggregate;
        }

        public async Task<QuizAggregate> HandleAsync(CreateQuizCommand command)
        {
            var packetAggregate = await _paquetRepository.GetPacketByNameAsync(command.PacketName);

            if (packetAggregate == null) 
                throw new EntityDoesNotExistException($"Packet with name {command.PacketName} does not exist, cannot create a Quiz.");
            
            if (!packetAggregate.RootEntity.CardsCollection.Any())
                throw new EmptyPaquetException($"Cannot create a Quiz with an empty Packet {command.PacketName}!");

            var currentCard = packetAggregate.RootEntity.CardsCollection
                .Where(x => x.LastQuiz != null)
                .MinBy(x => x.LastQuiz) ?? packetAggregate.RootEntity.CardsCollection.First();

            var quiz = new QuizEntity(currentCard.Id, packetAggregate.RootEntity.Name);
           
            var quizAggregate = new QuizAggregate(quiz, packetAggregate.RootEntity);

            await _quizRepository.SaveQuizAsync(quizAggregate);

            return quizAggregate;
        }

        public async Task<PacketAggregate> HandleAsync(AddCardToPacketCommand command)
        {
            var packetAggregate = await _paquetRepository.GetPacketByNameAsync(command.PacketName);

            if (packetAggregate == null)
                throw new EntityDoesNotExistException($"Packet with name {command.PacketName} does not exist, cannot add card to Packet.");

            var card = new CarteEntity(command.PacketName, command.Question, command.Answer, command.Score);

            var events = packetAggregate.AddCard(card);

            await _paquetRepository.SavePacketAsync(packetAggregate);

            foreach (var @event in events)
            {
                @event.Dispatch(_eventRouter);
            }

            return packetAggregate;
        }

        public async Task<Task> HandleAsync(DeletePaquetCommand command)
        {
            var packetAggregate = await _paquetRepository.GetPacketByNameAsync(command.Name);

            if (packetAggregate == null)
                throw new EntityDoesNotExistException($"Packet with name {command.Name} does not exist, cannot delete Packet.");

            await _paquetRepository.DeletePacketAsync(packetAggregate);
            return Task.CompletedTask;
        }
    }
}
