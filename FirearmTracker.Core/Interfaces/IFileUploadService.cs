namespace FirearmTracker.Core.Interfaces
{
    public interface IFileUploadService
    {
        Task<(bool success, string? fileName, string? errorMessage)> SaveFileAsync(
            Stream fileStream,
            string originalFileName,
            string contentType);

        string GetFilePath(string fileName);

        bool DeleteFile(string fileName);
    }
}