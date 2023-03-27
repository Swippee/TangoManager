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

        /// <summary>
        /// Requete pour ramener 1 seul paquet en fonction du nom
        /// </summary>
        /// <param name="name">nom du Paquet</param>
        /// <returns></returns>
        public async Task<PaquetEntity> GetPaquetByNameAsync(string name)
        {
            await using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

            await connection.OpenAsync();

            const string packetQuery = "select * from Paquet WHERE Name=@Name";
            var packetEntity = await connection.QueryFirstOrDefaultAsync<PaquetEntity>(packetQuery, new { Name = name });

            if (packetEntity == null) return null;

            const string cardsQuery = "select * from Carte WHERE PacketName=@Name";
            var cardEntities = await connection.QueryAsync<CarteEntity>(cardsQuery, new { Name = name });

            packetEntity.CardsCollection = cardEntities.ToList();

            return packetEntity;
        }

        /// <summary>
        /// Méthode pour créer un paquet
        /// </summary>
        /// <param name="paquet"></param>
        public async Task AddPaquetAsync(PaquetEntity paquet)
        {

            using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                var query = "Insert into Paquet (Name,Description,LastModification,LastQuiz) Values (@Name,@Description,GetDate(),NULL)";
                await connection.ExecuteAsync(query, paquet);
            }

        }
        /// <summary>
        /// Méthode pour mettre à jours la description d'un paquet
        /// </summary>
        /// <param name="paquet"></param>
        public async Task UpdatePaquetAsync(PaquetEntity paquet)
        {

            using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                var query = "Update Paquet Set Description=@Description Where Name=@Name";
                await connection.ExecuteAsync(query, paquet);
            }

        }

        /// <summary>
        /// Méthode pour créer un paquet
        /// </summary>
        /// <param name="paquet"></param>
        public async Task RemovePaquetAsync(string name)
        {

            using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                var query = "Delete from Paquet WHERE Name =@name";
                await connection.ExecuteAsync(query, new { name });
            }

        }
        /// <summary>
        /// Méthode pour créer un paquet
        /// </summary>
        /// <param name="paquet"></param>
        public  List<PaquetEntity> TestList()
        {

            using (SqlConnection connection = new(_config.GetConnectionString("DefaultConnection")))
            {
                var query = "select * from Paquet";
                var allTransaction =  connection.Query<PaquetEntity>(query);
                return allTransaction.ToList();
            }

        }


    }
}
