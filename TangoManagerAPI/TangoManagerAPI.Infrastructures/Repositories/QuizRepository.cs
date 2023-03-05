using Dapper;
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
            if (quizAggregate.RootEntity.Id == null)
            {
                // Faire le cas nouveau dans une transaction SQL
                // Ajouter dans les tables Quizz le nouveau Quizz, dans la table QUIzzCard toutes les cartes
                query = "Insert into ";
                // step a :
                // Insertion quiz
                // step b
                var quizId = await connection.ExecuteScalarAsync<int>(query);
                quizAggregate.RootEntity.Id = quizId;


            }
            else 
            {
                // Ajouter une quizCard (celle répondue)
                // Mettre à jour la table Quiz (dateModification, currentCardId,TotalScore,CurrentState)

                // Insert QuizCard depuis la collection AddedQuizCards
                // foreach 

                //Mettre à jours le paquet
                //Date du dernier quiz

                //Mettre à jours les cartes depuis answeredCard collection
                // via transaction applicatif
                //foreach 
            }

        }
    }
}
