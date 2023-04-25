using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using TangoManagerAPI.Application.Commands.CommandsAuth;
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
        ICommandHandler<PacketAggregate, AddCardToPacketCommand>,
        ICommandHandler<PacketLockEntity, LockPacketCommand>,
        ICommandHandler<Task, UnlockPacketCommand>
    {
        private readonly IPaquetRepository _packetRepository;
        private readonly IEventRouter _eventRouter;
        private readonly IQuizRepository _quizRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly IPacketLockRepository _packetLockRepository;

        public CommandHandler(IPaquetRepository packetRepository, IEventRouter eventRouter, IQuizRepository quizRepository, IMemoryCache memoryCache, IPacketLockRepository packetLockRepository)
        {
            _packetRepository = packetRepository;
            _eventRouter = eventRouter;
            _quizRepository = quizRepository;
            _memoryCache = memoryCache;
            _packetLockRepository= packetLockRepository;
        }

        public async Task<PacketAggregate> HandleAsync(CreatePaquetCommand command)
        {
            var packetAggregate = await _packetRepository.GetPacketByNameAsync(command.Name);

            if (packetAggregate != null)
                throw new EntityAlreadyExistsException($"Packet with name {packetAggregate.RootEntity.Name} already exists in base");

            var packetEntity = new PaquetEntity(command.Name, command.Description);

            packetAggregate = new PacketAggregate(packetEntity);

            await _packetRepository.SavePacketAsync(packetAggregate);

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
            var packetAggregate = await _packetRepository.GetPacketByNameAsync(command.PacketName);

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
            var packetAggregate = await _packetRepository.GetPacketByNameAsync(command.PacketName);

            if (packetAggregate == null)
                throw new EntityDoesNotExistException($"Packet with name {command.PacketName} does not exist, cannot add card to Packet.");

            var card = new CarteEntity(command.PacketName, command.Question, command.Answer, command.Score);

            var events = packetAggregate.AddCard(card);

            await _packetRepository.SavePacketAsync(packetAggregate);

            foreach (var @event in events)
            {
                @event.Dispatch(_eventRouter);
            }

            return packetAggregate;
        }

        public async Task<Task> HandleAsync(DeletePaquetCommand command)
        {
            var packetAggregate = await _packetRepository.GetPacketByNameAsync(command.Name);

            if (packetAggregate == null)
                throw new EntityDoesNotExistException($"Packet with name {command.Name} does not exist, cannot delete Packet.");

            await _packetRepository.DeletePacketAsync(packetAggregate);
            return Task.CompletedTask;
        }

        public async Task<PacketLockEntity> HandleAsync(LockPacketCommand command)
        {
            var packetLockEntity = new PacketLockEntity(command.PacketName, Guid.NewGuid().ToString());

             await _packetLockRepository.CreatePacketLockAsync(packetLockEntity);
            return _memoryCache.Set(command.PacketName, packetLockEntity, new MemoryCacheEntryOptions
            {
                SlidingExpiration = PacketLockEntity.CacheExpiration
            });
        }

        public async Task<Task> HandleAsync(UnlockPacketCommand command)
        {
            await _packetLockRepository.DeletePacketLockAsync(command.PacketName);

            _memoryCache.Remove(command.PacketName);

            return Task.CompletedTask;
        }
    }
}
