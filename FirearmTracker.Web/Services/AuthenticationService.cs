using FirearmTracker.Core.Interfaces;
using FirearmTracker.Core.Models;

namespace FirearmTracker.Web.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IWebHostEnvironment _environment;

        public AuthenticationService(IUserRepository userRepository, IWebHostEnvironment environment)  // Add this parameter
        {
            _userRepository = userRepository;
            _environment = environment;  // Add this assignment
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, passwordHash);
            }
            catch
            {
                return false;
            }
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);

            if (user == null || !user.IsActive)
                return null;

            if (!VerifyPassword(password, user.PasswordHash))
                return null;

            user.LastLoginDate = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            return user;
        }

        public async Task<User> RegisterFirstAdminAsync(string username, string email, string password)
        {
            if (await _userRepository.HasAnyUsersAsync())
            {
                throw new InvalidOperationException("Users already exist in the system.");
            }

            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = HashPassword(password),
                Role = UserRoles.Owner,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            return await _userRepository.CreateAsync(user);
        }

        public async Task<User> CreateUserAsync(string username, string email, string password, string role)
        {
            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = HashPassword(password),
                Role = role,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            return await _userRepository.CreateAsync(user);
        }

        public async Task<bool> IsFirstTimeSetupAsync()
        {
            return !await _userRepository.HasAnyUsersAsync();
        }
    }
}