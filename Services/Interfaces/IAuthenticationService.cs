using CustomPcStoreApp.Models;

namespace CustomPcStoreApp.Services.Interfaces;

/// <summary>
/// Результат аутентификации
/// </summary>
public class AuthResult
{
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public UserInfo? User { get; set; }
}

/// <summary>
/// Информация о пользователе
/// </summary>
public class UserInfo
{
    public int Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public int? EmployeeId { get; set; }
    public int? ClientId { get; set; }
    public string DisplayName { get; set; } = string.Empty;
}

/// <summary>
/// Интерфейс сервиса аутентификации
/// </summary>
public interface IAuthenticationService
{
    Task<AuthResult> LoginAsync(string username, string password);
    Task LogoutAsync();
    Task<bool> IsAuthenticatedAsync();
    Task<UserInfo?> GetCurrentUserAsync();
}