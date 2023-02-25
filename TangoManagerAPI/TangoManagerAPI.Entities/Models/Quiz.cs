using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangoManagerAPI.Entities.Exceptions;

namespace TangoManagerAPI.Entities.Models
{
    public class Quiz
    {
        public ICollection<CarteEntity> AnsweredCartes { get; }
        public ICollection<CarteEntity> IncorrectlyAnsweredCartes { get; }
        public ICollection<CarteEntity> CorrectlyAnsweredCartes { get; }

        public PaquetEntity Paquet { get; }

        public CarteEntity CurrentCarte { get; internal set; }

        public IQuizState QuizState { get; private set; }

        public decimal TotalScore { get; internal set; }


        public Quiz(PaquetEntity paquet)
        {
            if (!paquet.Cartes.Any())
                throw new EmptyPaquetException("Cannot create a Quiz with an empty Paquet!");

            Paquet = paquet;
            AnsweredCartes = new List<CarteEntity>();
            IncorrectlyAnsweredCartes = new List<CarteEntity>();
            CorrectlyAnsweredCartes = new List<CarteEntity>();
            CurrentCarte = Paquet.Cartes.First();
            QuizState = new QuizActiveState(this);
            TotalScore = 0m;
        }

        public void ChangeState(IQuizState state)
        {
            QuizState = state;
        }

        public void Answer(string answer)
        {
            QuizState.Answer(answer);
        }
    }
}
