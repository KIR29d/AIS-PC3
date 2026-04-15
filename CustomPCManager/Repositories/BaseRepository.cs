using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;

namespace CustomPCManager.Repositories
{
    /// <summary>
    /// Базовый класс для репозиториев с подключением к БД
    /// </summary>
    public abstract class BaseRepository
    {
        protected readonly string ConnectionString;

        protected BaseRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        protected IDbConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
