using FirearmTracker.Core.Models;

namespace FirearmTracker.Core.Interfaces
{
    public interface IBackupRestoreService
    {
        Task<byte[]> CreateBackupAsync(string currentUsername);

        Task<BackupMetadata> ValidateBackupAsync(Stream backupStream);

        Task RestoreBackupAsync(Stream backupStream);
    }
}