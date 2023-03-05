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
        ICommandHandler<PaquetEntity, CreatePaquetCommand>,
        ICommandHandler<QuizAggregate, AnswerQuizCommand>,
        ICommandHandler<QuizAggregate, CreateQuizCommand>
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

        public async Task<PaquetEntity> HandleAsync(CreatePaquetCommand command)
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

        public async Task<QuizAggregate> HandleAsync(AnswerQuizCommand command)
        {
            var quizAggregate = await _quizRepository.GetQuizByIdAsync(command.QuizId);

            await _quizRepository.SaveQuizAsync(quizAggregate);

            var events = quizAggregate.Answer(command.Answer);

            foreach (var @event in events)
            {
                @event.Dispatch(_eventRouter);
            }
            
            return quizAggregate;
        }

        public async Task<QuizAggregate> HandleAsync(CreateQuizCommand command)
        {
            var packet = await _paquetRepository.GetPaquetByNameAsync(command.PacketName);
            
            var currentCard = packet.CardsCollection
                .Where(x => x.DateDernierQuiz != null)
                .MinBy(x => x.DateDernierQuiz) ?? packet.CardsCollection.First();

            var quiz = new QuizEntity(currentCard.Id, packet.Nom);
            var quizAggregate = new QuizAggregate(quiz, packet);

            await _quizRepository.SaveQuizAsync(quizAggregate);

            return quizAggregate;
        }
    }
}
