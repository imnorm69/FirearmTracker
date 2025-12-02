using FirearmTracker.Core.Interfaces;

namespace FirearmTracker.Web.Services
{
    public class AvatarService(IUserRepository userRepository, IWebHostEnvironment environment) : IAvatarService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IWebHostEnvironment _environment = environment;

        public async Task<(byte[]? imageData, string? contentType)> GetUserAvatarAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user?.AvatarImage != null && user.AvatarContentType != null)
            {
                return (user.AvatarImage, user.AvatarContentType);
            }

            // Fall back to default avatar
            var defaultAvatarPath = Path.Combine(_environment.WebRootPath, "images", "defaults", "default-avatar.png");
            if (File.Exists(defaultAvatarPath))
            {
                var defaultImage = await File.ReadAllBytesAsync(defaultAvatarPath);
                return (defaultImage, "image/png");
            }

            return (null, null);
        }
    }
}