using TangoManagerAPI.Entities.Exceptions;
using TangoManagerAPI.Entities.Models;

namespace TangoManagerAPI.Entities.Tests
{
    public class QuizTests
    {
        [Fact]
        public void ShouldAnswer1CarteAndCheckProperties()
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
                DateCreation = DateTime.UtcNow,
                DateDernierQuiz = null,
                Description = "Book QuizAggregate",
                Nom = "Quiz1"
            };

            var quiz = QuizEntity.Create(carteEntity.Id, paquet.Nom);

            var quizAggregate = new QuizAggregate(quiz, paquet, new List<CarteEntity> {carteEntity});

            //When
            quizAggregate.Answer("alexandre dumas");

            //Then
            Assert.Contains(quizAggregate.CorrectlyAnsweredCards, x => x.Id == carteEntity.Id);
            Assert.Contains(quizAggregate.AnsweredCards, x => x.Id == carteEntity.Id);
            Assert.True(quizAggregate.QuizState is QuizFinishedState);
            Assert.True(quiz.ModificationDate != null);
            Assert.True(quiz.TotalScore == carteEntity.Score);
            Assert.True(carteEntity.DateDernierQuiz != null);
            Assert.True(paquet.DateDernierQuiz != null);
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
                DateCreation = DateTime.UtcNow,
                DateDernierQuiz = null,
                Description = "Book QuizAggregate",
                Nom = "Quiz1"
            };

            var quiz = QuizEntity.Create(carteEntity.Id, paquet.Nom);

            var quizAggregate = new QuizAggregate(quiz, paquet, new List<CarteEntity> {carteEntity});

            //Then
            Assert.Throws<QuizAlreadyFinishedException>(() =>
            {
                quizAggregate.Answer("alexandre dumas");
                quizAggregate.Answer("alexandre dumas");
            });
        }

        [Fact]
        public void ShouldThrowEmptyPaquetException()
        {
            //Given
            var paquet = new PaquetEntity
            {
                DateCreation = DateTime.UtcNow,
                DateDernierQuiz = null,
                Description = "Book QuizAggregate",
                Nom = "Quiz1"
            };

            //Then
            Assert.Throws<EmptyPaquetException>(() =>
            {
                var quiz = QuizEntity.Create(1, paquet.Nom);
                var _ = new QuizAggregate(quiz, paquet, Enumerable.Empty<CarteEntity>());
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
                DateCreation = DateTime.UtcNow,
                DateDernierQuiz = null,
                Description = "Book QuizAggregate",
                Nom = "Quiz1"
            };
            var quiz = QuizEntity.Create(carteEntity1.Id, paquet.Nom);
            var quizAggregate = new QuizAggregate(quiz, paquet, new List<CarteEntity> {carteEntity1, carteEntity2});

            //When
            quizAggregate.Answer("alexandre dumas");

            //Then
            Assert.True(quizAggregate.QuizCards.Count(x => !x.IsCorrect) == 0);
            Assert.True(quizAggregate.QuizCards.Count(x => x.IsCorrect) == 1);
            Assert.True(quizAggregate.CurrentCard.Equals(carteEntity2));
            Assert.True(quizAggregate.QuizState is QuizActiveState);
        }

    }
}