using CustomPcStoreApp.Services.Interfaces;

namespace CustomPcStoreApp.Services;

/// <summary>
/// Singleton для хранения информации о текущем пользователе
/// </summary>
public class UserSession
{
    private UserInfo? _currentUser;

    public UserInfo? CurrentUser
    {
        get => _currentUser;
        set
        {
            _currentUser = value;
            OnUserChanged?.Invoke(value);
        }
    }

    public bool IsAuthenticated => CurrentUser != null;

    public event Action<UserInfo?>? OnUserChanged;

    public void Clear()
    {
        CurrentUser = null;
    }
}