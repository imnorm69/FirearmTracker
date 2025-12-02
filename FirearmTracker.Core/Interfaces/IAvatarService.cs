namespace FirearmTracker.Core.Interfaces
{
    public interface IAvatarService
    {
        Task<(byte[]? imageData, string? contentType)> GetUserAvatarAsync(int userId);
    }
}