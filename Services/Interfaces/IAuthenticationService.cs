using System;
using System.Threading.Tasks;
using PCStoreApp.Models.Entities;

namespace PCStoreApp.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<User?> AuthenticateAsync(string login, string password);
        void Logout();
        User? CurrentUser { get; }
        bool IsAuthenticated { get; }
        event EventHandler<User?>? AuthenticationChanged;
    }
}