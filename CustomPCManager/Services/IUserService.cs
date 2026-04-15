using CustomPCManager.Models;

namespace CustomPCManager.Services
{
    /// <summary>
    /// Интерфейс сервиса для работы с пользователями
    /// </summary>
    public interface IUserService
    {
        Task<User?> GetByIdAsync(int id);
        Task<User?> AuthenticateAsync(string login, string password);
        Task<User> RegisterAsync(string login, string password, int roleId, int? employeeId = null, int? clientId = null);
        Task UpdateLastLoginAsync(int userId);
        Task<bool> HasRoleAsync(int userId, string roleName);
    }
}
