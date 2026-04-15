using System.Data;
using Dapper;
using CustomPCManager.Models;

namespace CustomPCManager.Repositories
{
    /// <summary>
    /// Репозиторий для работы с пользователями
    /// </summary>
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            using var connection = CreateConnection();
            const string sql = @"
                SELECT * FROM пользователи 
                WHERE пользователь_id = @Id";
            
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
        }

        public async Task<User?> GetByLoginAsync(string login)
        {
            using var connection = CreateConnection();
            const string sql = @"
                SELECT * FROM пользователи 
                WHERE логин = @Login";
            
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Login = login });
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            using var connection = CreateConnection();
            const string sql = "SELECT * FROM пользователи ORDER BY логин";
            
            return await connection.QueryAsync<User>(sql);
        }

        public async Task<int> CreateAsync(User user)
        {
            using var connection = CreateConnection();
            const string sql = @"
                INSERT INTO пользователи (логин, пароль, роль_id, сотрудник_id, клиент_id, активен, дата_создания)
                OUTPUT INSERTED.пользователь_id
                VALUES (@логин, @пароль, @роль_id, @сотрудник_id, @клиент_id, @активен, @дата_создания)";
            
            return await connection.ExecuteScalarAsync<int>(sql, user);
        }

        public async Task UpdateAsync(User user)
        {
            using var connection = CreateConnection();
            const string sql = @"
                UPDATE пользователи 
                SET логин = @логин, пароль = @пароль, роль_id = @роль_id, 
                    сотрудник_id = @сотрудник_id, клиент_id = @клиент_id, 
                    активен = @активен, дата_последнего_входа = @дата_последнего_входа
                WHERE пользователь_id = @пользователь_id";
            
            await connection.ExecuteAsync(sql, user);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = CreateConnection();
            const string sql = "DELETE FROM пользователи WHERE пользователь_id = @Id";
            
            await connection.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<User?> AuthenticateAsync(string login, string password)
        {
            using var connection = CreateConnection();
            const string sql = @"
                SELECT * FROM пользователи 
                WHERE логин = @Login AND пароль = @Password AND активен = 1";
            
            var user = await connection.QueryFirstOrDefaultAsync<User>(sql, new { Login = login, Password = password });
            
            if (user != null)
            {
                await UpdateLastLoginAsync(user.пользователь_id);
            }
            
            return user;
        }

        private async Task UpdateLastLoginAsync(int userId)
        {
            using var connection = CreateConnection();
            const string sql = @"
                UPDATE пользователи 
                SET дата_последнего_входа = @LastLogin 
                WHERE пользователь_id = @Id";
            
            await connection.ExecuteAsync(sql, new { LastLogin = DateTime.Now, Id = userId });
        }
    }
}
