using FirearmTracker.Core.Models;

namespace FirearmTracker.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);

        Task<User?> GetByUsernameAsync(string username);

        Task<IEnumerable<User>> GetAllAsync();

        Task<IEnumerable<User>> GetAllActiveAsync();

        Task<User> CreateAsync(User user);

        Task UpdateAsync(User user);

        Task DeleteAsync(int id);

        Task<bool> UsernameExistsAsync(string username);

        Task<bool> UsernameExistsAsync(string username, int excludeUserId);

        Task<bool> HasAnyUsersAsync();

        Task<int> GetUserCountAsync();
    }
}