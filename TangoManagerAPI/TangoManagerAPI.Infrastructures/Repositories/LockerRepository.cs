using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangoManagerAPI.Entities.Models;
using TangoManagerAPI.Entities.Ports.Repositories;

namespace TangoManagerAPI.Infrastructures.Repositories
{
    public class LockerRepository : ILockerRepository
    {
        private readonly IConfiguration _config;

        public LockerRepository (IConfiguration config)
        {
            _config = config;
        }

        public async Task<PacketLockEntity> CreateLockerAsync(string packetName)
        {
            string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

            await using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.OpenAsync();

            const string LockerQuery = @"
              BEGIN
                IF NOT EXISTS (SELECT TOP 1 PacketName FROM PacketLock WHERE PacketName=@Name)
                    BEGIN
                        INSERT INTO PacketLock (PacketName,UserLock,DateLock) Values (@Name,@Token,GetDate())
                    END
                ELSE
                   THROW 51000, 'There is already a Lock in place for table @Name', 1;  
              END";

           var test =await connection.ExecuteAsync(LockerQuery, new { Name = packetName, Token = token });
           
           var entityPacketLock = new PacketLockEntity() { PacketName=packetName,token= token};

            return entityPacketLock;

        }

    }
}
