using System;
using System.Collections.Generic;
using System.Linq;
using TangoManagerAPI.Entities.Events;
using TangoManagerAPI.Entities.Exceptions;

namespace TangoManagerAPI.Entities.Models
{
    public class QuizAggregate : IAggregateRoot<QuizEntity>
    {
        public CarteEntity CurrentCard { get; internal set; }

        public IQuizState CurrentState { get; internal set; }

        internal ICollection<CarteEntity> AnsweredCardsCollection { get; }
        public IEnumerable<CarteEntity> AnsweredCards => AnsweredCardsCollection;


        internal ICollection<CarteEntity> CorrectlyAnsweredCardsCollection { get; }
        public IEnumerable<CarteEntity> CorrectlyAnsweredCards => CorrectlyAnsweredCardsCollection;

        internal ICollection<CarteEntity> IncorrectlyAnsweredCardsCollection { get; }
        public IEnumerable<CarteEntity> IncorrectlyAnsweredCards => IncorrectlyAnsweredCardsCollection;

        internal ICollection<CarteEntity> PacketCardsCollection { get; }
        public IEnumerable<CarteEntity> PacketCards => PacketCardsCollection;

        internal ICollection<QuizCardEntity> QuizCardsCollection { get; }
        public IEnumerable<QuizCardEntity> QuizCards => QuizCardsCollection;


        public QuizAggregate(QuizEntity quiz, PaquetEntity packet, IEnumerable<CarteEntity> packetCards) : 
        this(quiz, packet, packetCards, Enumerable.Empty<QuizCardEntity>())
        {

        }

        public QuizAggregate(QuizEntity quiz, PaquetEntity packet, IEnumerable<CarteEntity> packetCards, IEnumerable<QuizCardEntity> quizCards)
        {
            PacketCardsCollection = packetCards.ToList();
            QuizCardsCollection = quizCards.ToList();
            RootEntity = quiz;
            packet.DateDernierQuiz = DateTime.UtcNow;

            if (!PacketCardsCollection.Any())
                throw new EmptyPaquetException("Cannot create a QuizAggregate with an empty Packet!");

            CurrentCard = PacketCardsCollection.FirstOrDefault(x => x.Id == quiz.CurrentCardId) ?? throw new CardNotFoundException($"Could not find Card with such Id {quiz.CurrentCardId} inside packet {packet.Nom}!");

            AnsweredCardsCollection = PacketCardsCollection.Where(x => QuizCardsCollection.Any(y => y.CardId == x.Id)).ToList();
            CorrectlyAnsweredCardsCollection = AnsweredCardsCollection.Where(x => QuizCardsCollection.Any(y => y.CardId == x.Id && y.IsCorrect)).ToList();
            IncorrectlyAnsweredCardsCollection = AnsweredCardsCollection.Where(x => QuizCardsCollection.Any(y => y.CardId == x.Id && !y.IsCorrect)).ToList();


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
            return Enumerable.Empty<AEvent>();
        }

        public QuizEntity RootEntity { get; }
    }
}
