﻿using System;
using System.Linq;
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

            _quizAggregate.AnsweredCardsCollection.Add(card);
            card.DateDernierQuiz = DateTime.UtcNow;
            _quizAggregate.EventsCollection.Add(new CardUpdatedEvent(card));

            if (string.Equals(card.Reponse, answer, StringComparison.InvariantCultureIgnoreCase))
            {
                var quizCardEntity = new QuizCardEntity(card.Id, _quizEntity.Id, true);
                _quizAggregate.CorrectlyAnsweredCardsCollection.Add(card);
                _quizAggregate.QuizCardsCollection.Add(quizCardEntity);
                _quizEntity.TotalScore += card.Score;
                _quizEntity.ModificationDate = DateTime.UtcNow;
                _quizAggregate.EventsCollection.Add(new QuizCardEntityAddedEvent(quizCardEntity));
            }
            else
            {
                var quizCardEntity = new QuizCardEntity(card.Id, _quizEntity.Id, false);
                _quizAggregate.IncorrectlyAnsweredCardsCollection.Add(card);
                _quizAggregate.QuizCardsCollection.Add(quizCardEntity);
                _quizAggregate.EventsCollection.Add(new QuizCardEntityAddedEvent(quizCardEntity));
            }

            var notAnsweredCards = _quizAggregate.PacketCardsCollection.Except(_quizAggregate.AnsweredCardsCollection).ToList();

            if (!notAnsweredCards.Any())
            {
                _quizAggregate.CurrentState = new QuizFinishedState();
                _quizEntity.CurrentState = _quizAggregate.CurrentState.ToString();
                _packetEntity.DateDernierQuiz = DateTime.UtcNow;
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
