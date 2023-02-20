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
    public class WriteRepository : IWriteRepository
    {
        private readonly IConfiguration _config;
        public WriteRepository(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Méthode pour créer un paquet
        /// </summary>
        /// <param name="paquet"></param>
        public async void AddPaquet(PaquetEntity paquet)
        {

            using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                var query = "Insert into Paquet (Nom,Description,Score,DateCreation,DateDernierQuiz) Values (@Nom,@Description,NULL,GetDate(),NULL)";
                connection.Execute(query, paquet);
            }

        }
        /// <summary>
        /// Méthode pour créer un paquet
        /// </summary>
        /// <param name="paquet"></param>
        public async void RemovePaquet(string name)
        {

            using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                await connection.OpenAsync();
                var query = "Delete from Paquet WHERE Nom =@name";
                connection.Execute(query, new {name});
            }

        }
    }
}
