using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using TangoManagerAPI.Entities.Exceptions;
using TangoManagerAPI.Entities.Models;
using TangoManagerAPI.Entities.Ports.Repositories;

namespace TangoManagerAPI.Infrastructures.Repositories
{
    public class PaquetRepository : IPaquetRepository
    {
        private readonly IConfiguration _config;
        public PaquetRepository(IConfiguration config)
        {
            _config = config;
        }
        /// <summary>
        /// Requete pour ramener tous les paquets dans une liste
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<PacketAggregate>> GetPacketsAsync()
        {
            List<PacketAggregate> result= new List<PacketAggregate>();
            using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                var query = "select * from Paquet";

                var packetstsEntity = await connection.QueryAsync<PaquetEntity>(query);

                const string cardsQuery = "select * from Carte WHERE PacketName=@Name";
                foreach (var packet in packetstsEntity)
                {

                    var cardEntities = await connection.QueryAsync<CarteEntity>(cardsQuery, new { Name = packet.Name });
                    packet.CardsCollection = cardEntities.ToList();
                    result.Add(new PacketAggregate(packet));
                }
                return result;
            }
        }

        /// <summary>
        /// Requete pour ramener 1 seul paquet en fonction du nom
        /// </summary>
        /// <param name="name">nom du Paquet</param>
        /// <returns></returns>
        public async Task<PacketAggregate?> GetPacketByNameAsync(string name)
        {
            await using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

            await connection.OpenAsync();

            const string packetQuery = "select * from Paquet WHERE Name=@Name";
            var packetEntity = await connection.QueryFirstOrDefaultAsync<PaquetEntity>(packetQuery, new { Name = name });

            if(packetEntity == null) return null; 
            const string cardsQuery = "select * from Carte WHERE PacketName=@Name";
            var cardEntities = await connection.QueryAsync<CarteEntity>(cardsQuery, new { Name = name });

            packetEntity.CardsCollection = cardEntities.ToList();

            var packetAggregate = new PacketAggregate(packetEntity);

            return packetAggregate;
        }

        
        public async Task SavePacketAsync(PacketAggregate packetAggregate)
        {
            await using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.OpenAsync();
            await using var sqlTran = (SqlTransaction)await connection.BeginTransactionAsync(IsolationLevel.Serializable);

            const string packetQuery = @"
              BEGIN
                IF NOT EXISTS (SELECT TOP 1 Name FROM PAQUET WHERE Name=@Name)
                    BEGIN
                        INSERT INTO PAQUET (Name,Description,LastModification,LastQuiz) Values (@Name,@Description,@LastModification,NULL)
                    END
                ELSE
                    BEGIN
                        UPDATE PAQUET SET 
                        Name=@Name,
                        Description=@Description,
                        LastModification=@LastModification,
                        LastQuiz=@LastQuiz
                        WHERE Name=@Name
                    END
              END";

            await using var packetCmd = new SqlCommand(packetQuery, connection, sqlTran);
            packetCmd.Parameters.AddWithValue("@Name", packetAggregate.RootEntity.Name);
            packetCmd.Parameters.AddWithValue("@Description", packetAggregate.RootEntity.Description);
            packetCmd.Parameters.AddWithValue("@LastModification", packetAggregate.RootEntity.LastModification);
            packetCmd.Parameters.Add(new SqlParameter("@LastQuiz", SqlDbType.DateTime)
            {
                Value = packetAggregate.RootEntity.LastQuiz ?? SqlDateTime.Null,
                IsNullable = true
            });

            const string insertCardQuery =
                @"BEGIN
                    INSERT INTO CARTE (PacketName, Question, Answer, Score, LastModification, LastQuiz)
                    VALUES (@PacketName, @Question, @Answer, @Score, @LastModification, @LastQuiz)
                    SELECT SCOPE_IDENTITY();
                END";

            await using var cardsCmd = new SqlCommand(insertCardQuery, connection, sqlTran);
            cardsCmd.Parameters.Add(new SqlParameter("@PacketName", SqlDbType.VarChar));
            cardsCmd.Parameters.Add(new SqlParameter("@Question", SqlDbType.VarChar));
            cardsCmd.Parameters.Add(new SqlParameter("@Answer", SqlDbType.VarChar));
            cardsCmd.Parameters.Add(new SqlParameter("@Score", SqlDbType.Decimal));
            cardsCmd.Parameters.Add(new SqlParameter("@LastModification", SqlDbType.DateTime));
            cardsCmd.Parameters.Add(new SqlParameter("@LastQuiz", SqlDbType.DateTime) { IsNullable = true });

            try 
            {
                await packetCmd.ExecuteNonQueryAsync();

                foreach (var addedCard in packetAggregate.AddedCards)
                {
                    cardsCmd.Parameters["@PacketName"].Value = addedCard.PacketName;
                    cardsCmd.Parameters["@Question"].Value = addedCard.Question;
                    cardsCmd.Parameters["@Answer"].Value = addedCard.Answer;
                    cardsCmd.Parameters["@Score"].Value = addedCard.Score;
                    cardsCmd.Parameters["@LastModification"].Value = addedCard.LastModification;
                    cardsCmd.Parameters["@LastQuiz"].Value = addedCard.LastQuiz ?? SqlDateTime.Null;

                    var cardIdTemp = await cardsCmd.ExecuteScalarAsync();
                    if (cardIdTemp == null)
                        throw new Exception("Error while returning ID of Card");
                    var cardId = Convert.ToInt32(cardIdTemp);
                    addedCard.Id = cardId;
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
        public async Task DeletePacketAsync(PacketAggregate packetAggregate)
        {

            await using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.OpenAsync();
            await using var sqlTran = (SqlTransaction)await connection.BeginTransactionAsync(IsolationLevel.Serializable);

            const string query = @"
              BEGIN
                DELETE quizCards
                FROM QuizCards quizCards 
                INNER JOIN QUIZ quiz ON quiz.Id=quizCards.IdQuiz
                INNER JOIN CARTE card ON card.PacketName=quiz.PacketName
                INNER JOIN PAQUET paquet ON paquet.Name=card.PacketName
                WHERE paquet.Name=@Name
              END";

            await using var packetCmd = new SqlCommand(query, connection, sqlTran);
            packetCmd.Parameters.AddWithValue("@Name", packetAggregate.RootEntity.Name);

            try
            {
                await packetCmd.ExecuteNonQueryAsync();
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
