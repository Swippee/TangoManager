using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
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

        public async Task<QuizAggregate> GetQuizByIdAsync(int id)
        {
            await using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.OpenAsync();

            var query = "select * from Quiz where Id=@Id";
            var quiz = await connection.QueryFirstOrDefaultAsync<QuizEntity>(query, new { Id = id });
           
            if (quiz == null)
                throw new EntityDoesNotExistException($"No quiz found with the Id {id}. Cannot restore Quiz state.");

            query = "select * from QuizCards where IdQuiz=@Id";
            var quizCards = await connection.QueryAsync<QuizCardEntity>(query, new { Id = id });

            quiz.QuizCardsCollection = quizCards.ToList();

            query = "select * from Paquet where Name=@Name";
            var packet = await connection.QueryFirstOrDefaultAsync<PaquetEntity>(query, new { Name = quiz.PacketName });

            if (packet == null)
                throw new EntityDoesNotExistException($"No packet found with the name {quiz.PacketName} for the quiz {quiz.Id}. Cannot restore Quiz state.");

            query = "select * from Carte where PacketName=@Name";
            var cards = await connection.QueryAsync<CarteEntity>(query, new { Name = quiz.PacketName });

            packet.CardsCollection = cards.ToList();

            var quizAggregate = new QuizAggregate(quiz, packet);

            return quizAggregate;
        }


        public async Task SaveQuizAsync(QuizAggregate quizAggregate)
        {
            await using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.OpenAsync();
            string query;

            if (quizAggregate.RootEntity.Id == null)
            {
                query =
                    @"BEGIN;
                          INSERT INTO Quiz (PacketName,CurrentCardId,CurrentState,CreationDate,LastModification,TotalScore) 
                          VALUES (@PacketName,@CurrentCardId,@CurrentState,GetDate(),GetDate(),0);
                          SELECT CAST(SCOPE_IDENTITY() as int);
                    END;";

                var quizId = await connection.ExecuteScalarAsync<int>(query, new { PacketName = quizAggregate.PacketEntity.Name, CurrentCardId = quizAggregate.CurrentCard.Id, CurrentState = quizAggregate.CurrentState.ToString() });
                quizAggregate.RootEntity.Id = quizId;
            }
            else
            {
                await using var sqlTran = (SqlTransaction)await connection.BeginTransactionAsync(IsolationLevel.Serializable);

                query = @"
                UPDATE Quiz 
                Set CurrentCardId =@CurrentCardId,
                CurrentState = @CurrentState,
                LastModification=@LastModification, 
                TotalScore=@TotalScore 
                WHERE PacketName=@PacketName;";

                await using var quizCmd = new SqlCommand(query, connection, sqlTran);
                quizCmd.Parameters.AddWithValue("@CurrentCardId", quizAggregate.RootEntity.CurrentCardId);
                quizCmd.Parameters.AddWithValue("@CurrentState", quizAggregate.RootEntity.CurrentState);
                quizCmd.Parameters.AddWithValue("@TotalScore", quizAggregate.RootEntity.TotalScore);
                quizCmd.Parameters.AddWithValue("@PacketName", quizAggregate.RootEntity.PacketName);
                quizCmd.Parameters.Add(new SqlParameter("@LastModification", SqlDbType.DateTime)
                {
                    Value = quizAggregate.RootEntity.LastModification ?? SqlDateTime.Null,
                    IsNullable = true
                });

                const string insertQuizCardQuery =
                @"BEGIN
                    INSERT INTO QuizCards (IdQuiz, IdCard, IsCorrect)
                    VALUES (@IdQuiz, @IdCard, @IsCorrect)
                END";

                await using var quizCardsCmd = new SqlCommand(insertQuizCardQuery, connection, sqlTran);
                quizCardsCmd.Parameters.Add(new SqlParameter("@IdQuiz", SqlDbType.Int));
                quizCardsCmd.Parameters.Add(new SqlParameter("@IdCard", SqlDbType.Int));
                quizCardsCmd.Parameters.Add(new SqlParameter("@IsCorrect", SqlDbType.Bit));

                try
                {
                    await quizCmd.ExecuteNonQueryAsync();
                    foreach (var quizCard in quizAggregate.AddedQuizCards)
                    {
                        quizCardsCmd.Parameters["@IdQuiz"].Value = quizAggregate.RootEntity.Id;
                        quizCardsCmd.Parameters["@IdCard"].Value = quizCard.IdCard;
                        quizCardsCmd.Parameters["@IsCorrect"].Value = quizCard.IsCorrect;
                        await quizCardsCmd.ExecuteNonQueryAsync();
                    }
                    sqlTran.Commit();
                }
                catch (Exception ex)
                {
                    await Console.Error.WriteLineAsync(ex.ToString());
                    sqlTran.Rollback();
                    throw;
                }
            }
        }
    }
}
