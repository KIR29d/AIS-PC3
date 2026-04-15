namespace CustomPCManager.Repositories
{
    /// <summary>
    /// Интерфейс репозитория для работы с пользователями
    /// </summary>
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByLoginAsync(string login);
        Task<IEnumerable<User>> GetAllAsync();
        Task<int> CreateAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
        Task<User?> AuthenticateAsync(string login, string password);
    }
}
