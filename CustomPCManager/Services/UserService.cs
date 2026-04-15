using System.Security.Cryptography;
using CustomPCManager.Models;
using CustomPCManager.Repositories;

namespace CustomPCManager.Services
{
    /// <summary>
    /// Сервис для работы с пользователями
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository? _roleRepository;

        public UserService(IUserRepository userRepository, IRoleRepository? roleRepository = null)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User?> AuthenticateAsync(string login, string password)
        {
            // Хеширование пароля
            var hashedPassword = HashPassword(password);
            return await _userRepository.AuthenticateAsync(login, hashedPassword);
        }

        public async Task<User> RegisterAsync(string login, string password, int roleId, int? employeeId = null, int? clientId = null)
        {
            // Проверка существования пользователя с таким логином
            var existing = await _userRepository.GetByLoginAsync(login);
            if (existing != null)
                throw new InvalidOperationException("Пользователь с таким логином уже существует");

            // Хеширование пароля
            var hashedPassword = HashPassword(password);

            var user = new User
            {
                логин = login,
                пароль = hashedPassword,
                роль_id = roleId,
                сотрудник_id = employeeId,
                клиент_id = clientId,
                активен = true,
                дата_создания = DateTime.Now
            };

            var userId = await _userRepository.CreateAsync(user);
            user.пользователь_id = userId;

            return user;
        }

        public async Task UpdateLastLoginAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"Пользователь с ID {userId} не найден");

            user.UpdateLastLogin();
            await _userRepository.UpdateAsync(user);
        }

        public async Task<bool> HasRoleAsync(int userId, string roleName)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return false;

            // TODO: Получить роль из репозитория и сравнить название
            // Пока заглушка
            return true;
        }

        /// <summary>
        /// Хеширование пароля с использованием SHA256
        /// В production следует использовать более надежные алгоритмы (bcrypt, PBKDF2)
        /// </summary>
        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
