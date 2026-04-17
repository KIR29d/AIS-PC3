using CustomPC.Core.Entities;
using CustomPC.Core.Interfaces;

namespace CustomPC.Core.Services;

/// <summary>
/// Сервис управления пользователями
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;

    public UserService(IUserRepository userRepository, IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public async Task<User?> RegisterAsync(string login, string password, int roleId)
    {
        // Проверка существования пользователя с таким логином
        var existingUser = await _userRepository.GetByLoginAsync(login);
        if (existingUser != null)
        {
            throw new InvalidOperationException($"Пользователь с логином '{login}' уже существует");
        }

        // Проверка существования роли
        var role = await _roleRepository.GetByIdAsync(roleId);
        if (role == null)
        {
            throw new InvalidOperationException($"Роль с ID {roleId} не найдена");
        }

        var user = new User
        {
            логин = login,
            пароль = password, // В реальной системе использовать хеширование
            роль_id = roleId,
            активен = true,
            дата_создания = DateTime.UtcNow
        };

        return await _userRepository.CreateAsync(user);
    }

    public async Task<User?> AuthenticateAsync(string login, string password)
    {
        var user = await _userRepository.GetByLoginAsync(login);
        
        if (user == null || !user.активен)
        {
            return null;
        }

        if (user.Authenticate(password))
        {
            await UpdateLastLoginAsync(user.пользователь_id);
            return user;
        }

        return null;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    public async Task UpdateLastLoginAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user != null)
        {
            user.UpdateLastLogin();
            await _userRepository.UpdateAsync(user);
        }
    }

    public async Task<bool> HasRoleAsync(int userId, string roleName)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return false;
        }

        var role = await _roleRepository.GetByNameAsync(roleName);
        if (role == null)
        {
            return false;
        }

        return user.роль_id == role.роль_id;
    }
}
