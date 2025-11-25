using FirearmTracker.Core.Models;

namespace FirearmTracker.Core.Interfaces
{
    public interface IAuthenticationService
    {
        string HashPassword(string password);

        bool VerifyPassword(string password, string passwordHash);

        Task<User?> AuthenticateAsync(string username, string password);

        Task<User> RegisterFirstAdminAsync(string username, string email, string password);

        Task<bool> IsFirstTimeSetupAsync();

        Task<User> CreateUserAsync(string username, string email, string password, string role);
    }
}