using Dapper;
using Dapper.Transaction;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangoManagerAPI.Entities.Exceptions;
using TangoManagerAPI.Entities.Models;
using TangoManagerAPI.Entities.Ports.Repositories;

namespace TangoManagerAPI.Infrastructures.Repositories
{
    public class QuizRepository : IQuizRepository
    {
        private readonly IConfiguration _config;
        public QuizRepository(IConfiguration config)
        {
            _config = config;
        }
        /// <summary>
        /// Requete pour ramener tous les paquets dans une liste
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<PaquetEntity>> GetPaquetsAsync()
        {

            using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                var query = "select * from Paquet";
                var allTransaction = await connection.QueryAsync<PaquetEntity>(query);
                return allTransaction.ToList();
            }
        }

        public async Task<QuizAggregate> GetQuizByIdAsync(int id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.OpenAsync();
            var query = "select * from Quiz where Id=@Id";
            var quiz = await connection.QueryFirstOrDefaultAsync<QuizEntity>(query, new { Id = id });
            if (quiz==null)
                throw new EntityDoNotExistException($"No quiz found with the Id {id}.");

            query = "select * from QuizCards where IdQuiz=@Id";
            var quizCards = await connection.QueryAsync<QuizCardEntity>(query, new { Id = id });

            quiz.QuizCardsCollection = quizCards.ToList();

            query = "select * from Paquet where Name=@Name";
            var packet = await connection.QueryFirstOrDefaultAsync<PaquetEntity>(query, new { Name = quiz.PacketName });
          
            if (packet == null)
                throw new EntityDoNotExistException($"No packet found with the name {quiz.PacketName}.");

            query = "select * from Carte where PacketName=@Name";
            var cards = await connection.QueryAsync<CarteEntity>(query, new { Name = quiz.PacketName });

            packet.CardsCollection=cards.ToList();

            await connection.CloseAsync();

            var quizAggregate = new QuizAggregate(quiz,packet);

            return quizAggregate;
        }


        public async Task SaveQuizAsync(QuizAggregate quizAggregate)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

            await connection.OpenAsync();
            string query = "";

            // IMPOSSIBLE CASE: Function GetQuizByIdAsync called before stops the process if quizId not found
            if (quizAggregate.RootEntity.Id == null)
            {
                using var transaction = await connection.BeginTransactionAsync();
                // Ajouter dans les tables Quizz le nouveau Quizz, dans la table QUIzzCard toutes les cartes
                query = "INSERT INTO Quiz (PacketName,CurrentCardId,CurrentState,CreationDate,LastModification,TotalScores) VALUES (@PacketName,@CurrentCardId,'ACTIVE',GetDate(),GetDate(),0); " +
                        "SELECT CAST(SCOPE_IDENTITY() as int)";

                var quizId = await transaction.ExecuteScalarAsync<int>(query, new { PacketName = quizAggregate.PacketEntity.Name, CurrentCardId = quizAggregate.CurrentCard.Id });
                quizAggregate.RootEntity.Id = quizId;

                query = "INSERT INTO QuizCards (IdQuiz,IdCard,IsCorrect) VALUES (@IdQuiz,@IdCard,@IsCorrect); ";
                await transaction.ExecuteAsync(query, new { IdQuiz = quizId, CurrentCardId = quizAggregate.RootEntity.QuizCardsCollection.Last().CardId, IsCorrect= quizAggregate.RootEntity.QuizCardsCollection.Last().IsCorrect?"Correct":"Incorrect" });
                await transaction.CommitAsync();
            }
            else
            {
                //{ TRANSACTION    APPLICATIVE 


                // Ajouter une quizCard (celle répondue) à la collection

                quizAggregate.AnsweredCards.ToList().Add(quizAggregate.CurrentCard);

                if (quizAggregate.RootEntity.QuizCardsCollection.Last().IsCorrect)
                    quizAggregate.CorrectlyAnsweredCards.ToList().Add(quizAggregate.CurrentCard);
                else quizAggregate.IncorrectlyAnsweredCards.ToList().Add(quizAggregate.CurrentCard);

                using var transaction = await connection.BeginTransactionAsync();
                query = "INSERT INTO QuizCards (IdQuiz,IdCard,IsCorrect) VALUES (@IdQuiz,@IdCard,@IsCorrect); ";
                await transaction.ExecuteAsync(query, new { IdQuiz = quizAggregate.RootEntity.Id, IdCard = quizAggregate.RootEntity.QuizCardsCollection.Last().CardId, IsCorrect = quizAggregate.RootEntity.QuizCardsCollection.Last().IsCorrect ? "Correct" : "Incorrect" });
                
                // Mettre à jour la table Quiz (dateModification, currentCardId,TotalScore,CurrentState)
                query = "UPDATE Quiz Set CurrentCardId =@CurrentCardId, LastModification=GetDate(), TotalScore=@TotalScore; ";
                await transaction.ExecuteAsync(query, new { CurrentCardId = quizAggregate.CurrentCard.Id, TotalScore = quizAggregate.RootEntity.QuizCardsCollection.Last().IsCorrect ? quizAggregate.RootEntity.TotalScore++: quizAggregate.RootEntity.TotalScore });

                query = "UPDATE Paquet Set LastQuiz =GetDate(); ";
                await transaction.ExecuteAsync(query);

                query = "UPDATE Carte Set LastQuiz =GetDate(),Score=@Score; ";
                await transaction.ExecuteAsync(query, new { Score = quizAggregate.RootEntity.QuizCardsCollection.Last().IsCorrect ? quizAggregate.CurrentCard.Score++ : quizAggregate.CurrentCard.Score });
               
                await transaction.CommitAsync();

            }
                //Mettre à jours le paquet
                //Date du dernier quiz

                //Mettre à jours les cartes depuis answeredCard collection
                // via transaction applicatif
                //foreach 
                //}
        }
    }
}
