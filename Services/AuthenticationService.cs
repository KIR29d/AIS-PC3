using CustomPcStoreApp.Services.Interfaces;

namespace CustomPcStoreApp.Services;

public class AuthenticationService : IAuthenticationService
{
    public Task<AuthResult> LoginAsync(string username, string password)
    {
        // Временная заглушка - будет реализовано в задаче 4
        throw new NotImplementedException();
    }

    public Task LogoutAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsAuthenticatedAsync()
    {
        throw new NotImplementedException();
    }

    public Task<UserInfo?> GetCurrentUserAsync()
    {
        throw new NotImplementedException();
    }
}