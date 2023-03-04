using System;
using System.Linq;

namespace TangoManagerAPI.Entities.Models
{
    public  class QuizActiveState : IQuizState
    {
        public static string StateName => "Active";

        private readonly QuizAggregate _quizAggregate;
        private readonly QuizEntity _quizEntity;

        public QuizActiveState(QuizAggregate quizAggregate)
        {
            _quizAggregate = quizAggregate;
            _quizEntity = quizAggregate.RootEntity;
        }

        public void Answer(string answer)
        {
            var card = _quizAggregate.CurrentCard;

            _quizAggregate.AnsweredCardsCollection.Add(card);
            card.DateDernierQuiz = DateTime.UtcNow;

            if (string.Equals(card.Reponse, answer, StringComparison.InvariantCultureIgnoreCase))
            {
                _quizAggregate.CorrectlyAnsweredCardsCollection.Add(card);
                _quizAggregate.QuizCardsCollection.Add(new QuizCardEntity(card.Id, _quizEntity.Id, true));
                _quizEntity.TotalScore += card.Score;
                _quizEntity.ModificationDate = DateTime.UtcNow;
            }
            else
            {
                _quizAggregate.IncorrectlyAnsweredCardsCollection.Add(card);
                _quizAggregate.QuizCardsCollection.Add(new QuizCardEntity(card.Id, _quizEntity.Id, false));
            }

            var notAnsweredCards = _quizAggregate.PacketCardsCollection.Except(_quizAggregate.AnsweredCardsCollection).ToList();

            if (!notAnsweredCards.Any())
            {
                _quizAggregate.CurrentState = new QuizFinishedState();
                _quizEntity.CurrentState = _quizAggregate.CurrentState.ToString();
            }
            else
            {
                _quizAggregate.CurrentCard = notAnsweredCards.First();
                _quizEntity.CurrentCardId = _quizAggregate.CurrentCard.Id;
            }
        }

        public override string ToString()
        {
            return StateName;
        }
    }
}
