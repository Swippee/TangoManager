using System;
using System.Linq;
using TangoManagerAPI.Entities.Events;
using TangoManagerAPI.Entities.Events.QuizAggregateEvents;

namespace TangoManagerAPI.Entities.Models
{
    public  class QuizActiveState : IQuizState
    {
        public static string StateName => "Active";

        private readonly QuizAggregate _quizAggregate;
        private readonly QuizEntity _quizEntity;
        private readonly PaquetEntity _packetEntity;

        public QuizActiveState(QuizAggregate quizAggregate)
        {
            _quizAggregate = quizAggregate;
            _quizEntity = quizAggregate.RootEntity;
            _packetEntity = quizAggregate.PacketEntity;
        }

        public void Answer(string answer)
        {
            var card = _quizAggregate.CurrentCard;
            if (_quizEntity.Id== null) 
            {
                throw new ArgumentNullException("The Quiz Id can't be NULL.");
            }
            card.LastQuiz = DateTime.UtcNow;
            _quizAggregate.AnsweredCardsCollection.Add(card);
            _quizAggregate.EventsCollection.Add(new CardUpdatedEvent(card));

            if (string.Equals(card.Answer, answer, StringComparison.InvariantCultureIgnoreCase))
            {
                var quizCardEntity = new QuizCardEntity(card.Id, _quizEntity.Id.Value, true);
                _quizAggregate.CorrectlyAnsweredCardsCollection.Add(card);
                _quizAggregate.AddedQuizCardsCollection.Add(quizCardEntity);
                _quizEntity.QuizCardsCollection.Add(quizCardEntity);
                _quizEntity.TotalScore += card.Score;
                _quizEntity.LastModification = DateTime.UtcNow;
                _quizAggregate.EventsCollection.Add(new QuizCardEntityAddedEvent(quizCardEntity));
            }
            else
            {
                var quizCardEntity = new QuizCardEntity(card.Id, _quizEntity.Id.Value, false);
                _quizAggregate.IncorrectlyAnsweredCardsCollection.Add(card);
                _quizAggregate.AddedQuizCardsCollection.Add(quizCardEntity);
                _quizEntity.QuizCardsCollection.Add(quizCardEntity);
                _quizAggregate.EventsCollection.Add(new QuizCardEntityAddedEvent(quizCardEntity));
            }

            var notAnsweredCards = _packetEntity.CardsCollection.Except(_quizAggregate.AnsweredCardsCollection).ToList();

            if (!notAnsweredCards.Any())
            {
                _quizAggregate.CurrentState = new QuizFinishedState();
                _quizEntity.CurrentState = _quizAggregate.CurrentState.ToString();
                _packetEntity.LastQuiz = DateTime.UtcNow;
                _quizAggregate.EventsCollection.Add(new PacketUpdatedEvent(_packetEntity));
            }
            else
            {
                _quizAggregate.CurrentCard = notAnsweredCards.First();
                _quizEntity.CurrentCardId = _quizAggregate.CurrentCard.Id;
            }

            _quizAggregate.EventsCollection.Add(new QuizAnsweredEvent(_quizEntity));
        }

        public override string ToString()
        {
            return StateName;
        }
    }
}
