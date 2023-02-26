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
using TangoManagerAPI.Entities.Ports.Repository;
using TangoManagerAPI.Models;

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

            using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                var query = "select * from Paquet WHERE Nom=@Name";
                var allTransaction = await connection.QueryFirstOrDefaultAsync<PaquetEntity>(query, new { Name = name });
                return allTransaction;
            }

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
                var query = "Insert into Paquet (Nom,Description,Score,DateCreation,DateDernierQuiz) Values (@Nom,@Description,NULL,GetDate(),NULL)";
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
                var query = "Update Paquet Set Description=@Description Where Nom=@Nom";
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
                var query = "Delete from Paquet WHERE Nom =@name";
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
