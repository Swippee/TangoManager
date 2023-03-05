using System;
using System.Collections.Generic;
using System.Linq;
using TangoManagerAPI.Entities.Events;
using TangoManagerAPI.Entities.Exceptions;

namespace TangoManagerAPI.Entities.Models
{
    public class QuizAggregate : IAggregateRoot<QuizEntity>
    {
        public IQuizState CurrentState { get; internal set; }

        internal ICollection<CarteEntity> AnsweredCardsCollection { get; }
        public IEnumerable<CarteEntity> AnsweredCards => AnsweredCardsCollection;

        internal ICollection<CarteEntity> CorrectlyAnsweredCardsCollection { get; }
        public IEnumerable<CarteEntity> CorrectlyAnsweredCards => CorrectlyAnsweredCardsCollection;

        internal ICollection<CarteEntity> IncorrectlyAnsweredCardsCollection { get; }
        public IEnumerable<CarteEntity> IncorrectlyAnsweredCards => IncorrectlyAnsweredCardsCollection;

        public IEnumerable<QuizCardEntity> AddedQuizCards => AddedQuizCardsCollection;
        internal ICollection<QuizCardEntity> AddedQuizCardsCollection {get; }

        public CarteEntity CurrentCard { get; internal set; }
        public PaquetEntity PacketEntity { get; }

        internal ICollection<AEvent> EventsCollection { get; }

        public QuizEntity RootEntity { get; }

        public QuizAggregate(QuizEntity quiz, PaquetEntity packetEntity)
        {
            EventsCollection = new List<AEvent>();
            AddedQuizCardsCollection = new List<QuizCardEntity>();
            RootEntity = quiz;
            PacketEntity = packetEntity;

            if (!packetEntity.CardsCollection.Any())
                throw new EmptyPaquetException($"Cannot create a Quiz with an empty Packet {packetEntity.Name}!");

            CurrentCard = packetEntity.CardsCollection.FirstOrDefault(x => x.Id == quiz.CurrentCardId) ?? throw new CardNotFoundException($"Could not find Card with such Id {quiz.CurrentCardId} inside packet {packetEntity.Name}!");

            AnsweredCardsCollection = packetEntity.CardsCollection.Where(x => RootEntity.QuizCardsCollection.Any(y => y.CardId == x.Id)).ToList();
            CorrectlyAnsweredCardsCollection = AnsweredCardsCollection.Where(x => RootEntity.QuizCardsCollection.Any(y => y.CardId == x.Id && y.IsCorrect)).ToList();
            IncorrectlyAnsweredCardsCollection = AnsweredCardsCollection.Where(x => RootEntity.QuizCardsCollection.Any(y => y.CardId == x.Id && !y.IsCorrect)).ToList();

            if (string.IsNullOrEmpty(quiz.CurrentState))
                throw new QuizInvalidStateException("Quiz state cannot be null or empty!");
            if (string.Equals(quiz.CurrentState, QuizActiveState.StateName, StringComparison.OrdinalIgnoreCase))
                CurrentState = new QuizActiveState(this);
            else if (string.Equals(quiz.CurrentState, QuizFinishedState.StateName, StringComparison.OrdinalIgnoreCase))
                CurrentState = new QuizFinishedState();
            else throw new QuizInvalidStateException($"Could not parse state {quiz.CurrentState}");
        }

        public IEnumerable<AEvent> Answer(string answer)
        {
            CurrentState.Answer(answer);
            return EventsCollection;
        }
    }
}
