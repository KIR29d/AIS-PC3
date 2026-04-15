using System.Data;
using Dapper;
using CustomPCManager.Models;

namespace CustomPCManager.Repositories
{
    /// <summary>
    /// Репозиторий для работы с клиентами
    /// </summary>
    public class ClientRepository : BaseRepository, IClientRepository
    {
        public ClientRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<Client?> GetByIdAsync(int id)
        {
            using var connection = CreateConnection();
            const string sql = @"
                SELECT * FROM клиенты 
                WHERE клиент_id = @Id";
            
            return await connection.QueryFirstOrDefaultAsync<Client>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            using var connection = CreateConnection();
            const string sql = "SELECT * FROM клиенты ORDER BY фамилия, имя";
            
            return await connection.QueryAsync<Client>(sql);
        }

        public async Task<int> CreateAsync(Client client)
        {
            using var connection = CreateConnection();
            const string sql = @"
                INSERT INTO клиенты (тип_клиента, фамилия, имя, отчество, наименование_организации, email, номер_телефона, дата_регистрации)
                OUTPUT INSERTED.клиент_id
                VALUES (@тип_клиента, @фамилия, @имя, @отчество, @наименование_организации, @email, @номер_телефона, @дата_регистрации)";
            
            return await connection.ExecuteScalarAsync<int>(sql, client);
        }

        public async Task UpdateAsync(Client client)
        {
            using var connection = CreateConnection();
            const string sql = @"
                UPDATE клиенты 
                SET тип_клиента = @тип_клиента, фамилия = @фамилия, имя = @имя, 
                    отчество = @отчество, наименование_организации = @наименование_организации,
                    email = @email, номер_телефона = @номер_телефона
                WHERE клиент_id = @клиент_id";
            
            await connection.ExecuteAsync(sql, client);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = CreateConnection();
            const string sql = "DELETE FROM клиенты WHERE клиент_id = @Id";
            
            await connection.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<IEnumerable<Client>> SearchAsync(string searchTerm)
        {
            using var connection = CreateConnection();
            const string sql = @"
                SELECT * FROM клиенты 
                WHERE фамилия LIKE @Search OR имя LIKE @Search OR 
                      отчество LIKE @Search OR email LIKE @Search OR 
                      номер_телефона LIKE @Search OR 
                      наименование_организации LIKE @Search
                ORDER BY фамилия, имя";
            
            var searchPattern = $"%{searchTerm}%";
            return await connection.QueryAsync<Client>(sql, new { Search = searchPattern });
        }
    }
}
