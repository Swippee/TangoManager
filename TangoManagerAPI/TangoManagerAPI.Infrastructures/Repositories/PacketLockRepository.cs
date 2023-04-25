#nullable enable
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using TangoManagerAPI.Entities.Models;
using TangoManagerAPI.Entities.Ports.Repositories;

namespace TangoManagerAPI.Infrastructures.Repositories
{
    public class PacketLockRepository : IPacketLockRepository
    {
        private readonly IConfiguration _config;
        public PacketLockRepository(IConfiguration config)
        {
            _config = config;
        }
        public async Task<PacketLockEntity> GetAsync(string packetName)
        {

            await using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

            await connection.OpenAsync();

            const string packetLockQuery = "select * from PacketLock WHERE PacketName=@PacketName";
            var packetLockEntity = await connection.QueryFirstOrDefaultAsync<PacketLockEntity>(packetLockQuery, new { PacketName = packetName });

            if (packetLockEntity == null)
                return null;

            return packetLockEntity;
        }
        public async Task CreatePacketLockAsync(PacketLockEntity packetLockEntity)
        {

            await using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.OpenAsync();
            await using var sqlTran = (SqlTransaction)await connection.BeginTransactionAsync(IsolationLevel.Serializable);
            const string query = @"
              BEGIN 
                IF NOT EXISTS (SELECT TOP 1 PacketName  FROM PacketLock pl WITH (ROWLOCK,UPDLOCK)
                                INNER JOIN Paquet pq on pq.Name= pl.PacketName and pl.PacketName=@PacketName )                              
                                
                    BEGIN
                        INSERT INTO PacketLock WITH (ROWLOCK,UPDLOCK) (PacketName,LockToken,CreationDateTime,LastAccessedDateTime) Values (@PacketName,@LockToken,@CreationDateTime,@LastAccessedDateTime) 
                    END
                ELSE
                   THROW 51000, 'There is already a Lock in place for table @PacketName', 1;  
              END";

            await using var packetLockCmd = new SqlCommand(query, connection, sqlTran);
            packetLockCmd.Parameters.AddWithValue("@PacketName", packetLockEntity.PacketName);
            packetLockCmd.Parameters.AddWithValue("@LockToken", packetLockEntity.LockToken);
            packetLockCmd.Parameters.AddWithValue("@CreationDateTime", packetLockEntity.CreationDateTime);
            packetLockCmd.Parameters.AddWithValue("@LastAccessedDateTime", packetLockEntity.LastAccessedDateTime);


            try
            {
                await packetLockCmd.ExecuteNonQueryAsync();
                sqlTran.Commit();
            }
            catch (Exception ex)
            {
                if (ex is SqlException sqlEx && sqlEx.Number== 51000)
                {
                    await Console.Error.WriteLineAsync(sqlEx.ToString());
                    sqlTran.Rollback();
                    throw new NotImplementedException(sqlEx.Message);
                }
                await Console.Error.WriteLineAsync(ex.ToString());
                sqlTran.Rollback();
                throw;
            }

        }
        public async Task DeletePacketLockAsync(string packetName)
        {
            await using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.OpenAsync();
            await using var sqlTran = (SqlTransaction)await connection.BeginTransactionAsync(IsolationLevel.Serializable);
            const string query = @"
              BEGIN 
                        DELETE FROM PacketLock WITH (ROWLOCK,UPDLOCK)  WHERE PacketName=@PacketName
              END";

            await using var packetUnlockCmd = new SqlCommand(query, connection, sqlTran);
            packetUnlockCmd.Parameters.AddWithValue("@PacketName", packetName);

            try
            {
                await packetUnlockCmd.ExecuteNonQueryAsync();
                sqlTran.Commit();
            }
            catch (Exception ex)
            {
                await Console.Error.WriteLineAsync(ex.ToString());
                sqlTran.Rollback();
                throw;
            }
        }

        public async Task UpdatePacketLockAsync(PacketLockEntity packetLockEntity)
        {
            await using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.OpenAsync();
            await using var sqlTran = (SqlTransaction)await connection.BeginTransactionAsync(IsolationLevel.Serializable);
            const string query = @"
              BEGIN 

                Update PacketLock WITH (ROWLOCK,UPDLOCK) 
                SET LastAccessedDateTime = @LastAccessedDateTime
                WHERE PacketName=@PacketName
 
              END";

            await using var packetLockCmd = new SqlCommand(query, connection, sqlTran);
            packetLockCmd.Parameters.AddWithValue("@PacketName", packetLockEntity.PacketName);
            packetLockCmd.Parameters.AddWithValue("@LastAccessedDateTime", packetLockEntity.LastAccessedDateTime);

            try
            {
                await packetLockCmd.ExecuteNonQueryAsync();
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
