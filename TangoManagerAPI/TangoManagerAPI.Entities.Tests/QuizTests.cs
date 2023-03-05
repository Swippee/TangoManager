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
                Answer = "Alexandre Dumas",
                LastModification = DateTime.UtcNow,
                Id = 1,
                LastQuiz = null,
                PacketName = "Famille",
                Score = 10.0m,
                Question = "The Three Musketeers"
            };

            var paquet = new PaquetEntity
            {
                LastModification = DateTime.UtcNow,
                LastQuiz = null,
                Description = "Book QuizAggregate",
                Name = "Quiz1",
                CardsCollection = { carteEntity }
            };

            var quiz = new QuizEntity(carteEntity.Id, paquet.Name);

            var quizAggregate = new QuizAggregate(quiz, paquet);

            //When
            quizAggregate.Answer("alexandre dumas");

            //Then
            Assert.Contains(quizAggregate.CorrectlyAnsweredCards, x => x.Id == carteEntity.Id);
            Assert.Contains(quizAggregate.AnsweredCards, x => x.Id == carteEntity.Id);
            Assert.True(quizAggregate.CurrentState is QuizFinishedState);
            Assert.True(quiz.LastModification != null);
            Assert.True(quiz.TotalScore == carteEntity.Score);
            Assert.True(carteEntity.LastQuiz != null);
            Assert.True(paquet.LastQuiz != null);
        }


        [Fact]
        public void ShouldThrowQuizAlreadyFinishedExceptionCarte()
        {
            //Given
            var carteEntity = new CarteEntity
            {
                Answer = "Alexandre Dumas",
                LastModification = DateTime.UtcNow,
                Id = 1,
                LastQuiz = null,
                PacketName = "Artiste",
                Score = 10.0m,
                Question = "The Three Musketeers"
            };

            var paquet = new PaquetEntity
            {
                LastModification = DateTime.UtcNow,
                LastQuiz = null,
                Description = "Book QuizAggregate",
                Name = "Quiz1",
                CardsCollection = { carteEntity }
            };

            var quiz = new QuizEntity(carteEntity.Id, paquet.Name);

            var quizAggregate = new QuizAggregate(quiz, paquet);

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
                LastModification = DateTime.UtcNow,
                LastQuiz = null,
                Description = "Book QuizAggregate",
                Name = "Quiz1"
            };

            //Then
            Assert.Throws<EmptyPaquetException>(() =>
            {
                var quiz = new QuizEntity(1, paquet.Name);
                var _ = new QuizAggregate(quiz, paquet);
            });
        }


        [Fact]
        public void ShouldAnswer1CarteAndCheckCurrentCarteAndCheckCurrentState()
        {
            //Given
            var carteEntity1 = new CarteEntity
            {
                Answer = "Alexandre Dumas",
                LastModification = DateTime.UtcNow,
                Id = 1,
                LastQuiz = null,
                PacketName = "Famille", 
                Score = 10.0m,
                Question = "The Three Musketeers"
            };

            var carteEntity2 = new CarteEntity
            {
                Answer = "Fyodor Dostoyevsky",
                LastModification = DateTime.UtcNow,
                Id = 2,
                LastQuiz = null,
                PacketName = "Famille",
                Score = 10.0m,
                Question = "Crime and Punishment"
            };

            var paquet = new PaquetEntity
            {
                LastModification = DateTime.UtcNow,
                LastQuiz = null,
                Description = "Book QuizAggregate",
                Name = "Quiz1",
                CardsCollection = {carteEntity1, carteEntity2}
            };
            var quiz = new QuizEntity(carteEntity1.Id, paquet.Name);
            var quizAggregate = new QuizAggregate(quiz, paquet);

            //When
            quizAggregate.Answer("alexandre dumas");

            //Then
            Assert.True(quizAggregate.RootEntity.QuizCardsCollection.Count(x => !x.IsCorrect) == 0);
            Assert.True(quizAggregate.RootEntity.QuizCardsCollection.Count(x => x.IsCorrect) == 1);
            Assert.True(quizAggregate.CurrentCard.Equals(carteEntity2));
            Assert.True(quizAggregate.CurrentState is QuizActiveState);
        }

    }
}