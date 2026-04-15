using CustomPCManager.Models;
using CustomPCManager.Services;

namespace CustomPCManager.ViewModels
{
    /// <summary>
    /// ViewModel для окна аутентификации
    /// </summary>
    public class LoginViewModel : BaseViewModel
    {
        private readonly IUserService _userService;
        private string _login = string.Empty;
        private string _password = string.Empty;
        private string? _errorMessage;
        private bool _isAuthenticated;

        public LoginViewModel(IUserService userService)
        {
            _userService = userService;
        }

        public string Login
        {
            get => _login;
            set => SetProperty(ref _login, value, nameof(Login));
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value, nameof(Password));
        }

        public string? ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value, nameof(ErrorMessage));
        }

        public bool IsAuthenticated
        {
            get => _isAuthenticated;
            set => SetProperty(ref _isAuthenticated, value, nameof(IsAuthenticated));
        }

        public async Task<bool> LoginAsync()
        {
            ErrorMessage = null;

            if (string.IsNullOrWhiteSpace(Login))
            {
                ErrorMessage = "Введите логин";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Введите пароль";
                return false;
            }

            try
            {
                var user = await _userService.AuthenticateAsync(Login, Password);
                
                if (user != null)
                {
                    IsAuthenticated = true;
                    CurrentUser = user;
                    return true;
                }
                else
                {
                    ErrorMessage = "Неверный логин или пароль";
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка входа: {ex.Message}";
                return false;
            }
        }

        // Статическое свойство для текущего пользователя
        public static User? CurrentUser { get; private set; }
    }
}
