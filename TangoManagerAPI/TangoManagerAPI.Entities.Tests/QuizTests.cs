using TangoManagerAPI.Entities.Exceptions;
using TangoManagerAPI.Entities.Models;

namespace TangoManagerAPI.Entities.Tests
{
    public class QuizTests
    {
        [Fact]
        public void ShouldAnswer1CarteAndCheckState()
        {
            //Given
            var carteEntity = new CarteEntity
            {
                Reponse = "Alexandre Dumas",
                DateCreation = DateTime.UtcNow,
                Id = 1,
                DateDernierQuiz = null,
                PaquetNom = "Famille",
                Score = 10.0m,
                Question = "The Three Musketeers"
            };

            var paquet = new PaquetEntity
            {
                Cartes = new List<CarteEntity> { carteEntity },
                DateCreation = DateTime.UtcNow,
                DateDernierQuiz = null,
                Description = "Book Quiz",
                Nom = "Quiz1"
            };

            var quiz = new Quiz(paquet);

            //When
            quiz.Answer("alexandre dumas");

            //Then
            Assert.True(quiz.CorrectlyAnsweredCartes.Contains(carteEntity));
            Assert.True(quiz.QuizState is QuizFinishedState);
        }


        [Fact]
        public void ShouldThrowQuizAlreadyFinishedExceptionCarte()
        {
            //Given
            var carteEntity = new CarteEntity
            {
                Reponse = "Alexandre Dumas",
                DateCreation = DateTime.UtcNow,
                Id = 1,
                DateDernierQuiz = null,
                PaquetNom = "Artiste",
                Score = 10.0m,
                Question = "The Three Musketeers"
            };

            var paquet = new PaquetEntity
            {
                Cartes = new List<CarteEntity> { carteEntity },
                DateCreation = DateTime.UtcNow,
                DateDernierQuiz = null,
                Description = "Book Quiz",
                Nom = "Quiz1"
            };

            var quiz = new Quiz(paquet);

            //Then
            Assert.Throws<QuizAlreadyFinishedException>(() =>
            {
                quiz.Answer("alexandre dumas");
                quiz.Answer("alexandre dumas");
            });
        }

        [Fact]
        public void ShouldThrowEmptyPaquetException()
        {
            //Given
            var paquet = new PaquetEntity
            {
                Cartes = new List<CarteEntity>(),
                DateCreation = DateTime.UtcNow,
                DateDernierQuiz = null,
                Description = "Book Quiz",
                Nom = "Quiz1"
            };

            //Then
            Assert.Throws<EmptyPaquetException>(() =>
            {
                var _ = new Quiz(paquet);
            });
        }


        [Fact]
        public void ShouldAnswer1CarteAndCheckCurrentCarteAndCheckCurrentState()
        {
            //Given
            var carteEntity1 = new CarteEntity
            {
                Reponse = "Alexandre Dumas",
                DateCreation = DateTime.UtcNow,
                Id = 1,
                DateDernierQuiz = null,
                PaquetNom = "Famille", 
                Score = 10.0m,
                Question = "The Three Musketeers"
            };

            var carteEntity2 = new CarteEntity
            {
                Reponse = "Fyodor Dostoyevsky",
                DateCreation = DateTime.UtcNow,
                Id = 2,
                DateDernierQuiz = null,
                PaquetNom = "Famille",
                Score = 10.0m,
                Question = "Crime and Punishment"
            };

            var paquet = new PaquetEntity
            {
                Cartes = new List<CarteEntity> { carteEntity1, carteEntity2 },
                DateCreation = DateTime.UtcNow,
                DateDernierQuiz = null,
                Description = "Book Quiz",
                Nom = "Quiz1"
            };

            var quiz = new Quiz(paquet);

            //When
            quiz.Answer("alexandre dumas");

            //Then
            Assert.True(quiz.CorrectlyAnsweredCartes.Contains(carteEntity1));
            Assert.True(quiz.IncorrectlyAnsweredCartes.Count == 0);
            Assert.True(quiz.AnsweredCartes.Count == 1);
            Assert.True(quiz.CurrentCarte.Equals(carteEntity2));
            Assert.True(quiz.QuizState is QuizActiveState);
        }

    }
}