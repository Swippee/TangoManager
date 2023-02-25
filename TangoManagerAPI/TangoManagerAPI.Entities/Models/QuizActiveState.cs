using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangoManagerAPI.Entities.Exceptions;

namespace TangoManagerAPI.Entities.Models
{
    public  class QuizActiveState : IQuizState
    {
        private readonly Quiz _quiz;

        public QuizActiveState(Quiz quiz)
        {
            _quiz = quiz;
        }

        public void Answer(string answer)
        {
            var carteEntity = _quiz.Paquet.Cartes.First(x => x.Equals(_quiz.CurrentCarte));

            if (string.Equals(carteEntity.Reponse, answer, StringComparison.InvariantCultureIgnoreCase))
            {
                _quiz.CorrectlyAnsweredCartes.Add(carteEntity);
            }
            else
            {
                _quiz.IncorrectlyAnsweredCartes.Add(carteEntity);
            }

            _quiz.AnsweredCartes.Add(carteEntity);
            _quiz.CurrentCarte = _quiz.Paquet.Cartes.Except(_quiz.AnsweredCartes).FirstOrDefault() ?? _quiz.CurrentCarte;
            _quiz.TotalScore += carteEntity.Score;

            if (!_quiz.Paquet.Cartes.Except(_quiz.AnsweredCartes).Any())
                _quiz.ChangeState(new QuizFinishedState());
        }
    }
}
