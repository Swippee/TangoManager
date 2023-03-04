using System.Threading.Tasks;
using TangoManagerAPI.Entities.Events.QuizAggregateEvents;
using TangoManagerAPI.Entities.Ports.Handlers;
using TangoManagerAPI.Entities.Ports.Repositories;

namespace TangoManagerAPI.Infrastructures.Handlers
{
    public class EventsHandler : 
        IEventHandler<QuizAnsweredEvent>, 
        IEventHandler<QuizCardEntityAddedEvent>,
        IEventHandler<PacketUpdatedEvent>,
        IEventHandler<CardUpdatedEvent>,
        IEventHandler<QuizCreatedEvent>
    {
        private readonly IQuizRepository _quizRepository;
        private readonly ICartesRepository _cartesRepository;
        private readonly IPaquetRepository _paquetRepository;

        public EventsHandler(IQuizRepository quizRepository, ICartesRepository cartesRepository, IPaquetRepository paquetRepository)
        {
            _quizRepository = quizRepository;
            _cartesRepository = cartesRepository;
            _paquetRepository = paquetRepository;
        }

        public async Task HandleAsync(QuizAnsweredEvent @event)
        {
            await _quizRepository.SaveQuizAsync(@event.Data);
        }

        public async Task HandleAsync(QuizCardEntityAddedEvent @event)
        {
            await _quizRepository.SaveQuizCard(@event.Data);
        }

        public async Task HandleAsync(PacketUpdatedEvent @event)
        {
            await _paquetRepository.SavePacketAsync(@event.Data);
        }

        public async Task HandleAsync(CardUpdatedEvent @event)
        {
            await _cartesRepository.SaveCardAsync(@event.Data);
        }

        public async Task HandleAsync(QuizCreatedEvent @event)
        {
            await _quizRepository.SaveQuizAsync(@event.Data);
        }
    }
}
