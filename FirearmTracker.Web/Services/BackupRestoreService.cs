using FirearmTracker.Core;
using FirearmTracker.Core.Interfaces;
using FirearmTracker.Core.Models;
using FirearmTracker.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;
using System.Text.Json;

namespace FirearmTracker.Web.Services
{
    public class BackupRestoreService : IBackupRestoreService
    {
        private readonly FirearmTrackerContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<BackupRestoreService> _logger;

        public BackupRestoreService(
            FirearmTrackerContext context,
            IWebHostEnvironment environment,
            ILogger<BackupRestoreService> logger)
        {
            _context = context;
            _environment = environment;
            _logger = logger;
        }

        public async Task<byte[]> CreateBackupAsync(string currentUsername)
        {
            _logger.LogInformation("Creating backup requested by user: {Username}", currentUsername);

            using var memoryStream = new MemoryStream();
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                // Create metadata
                var metadata = new BackupMetadata
                {
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = currentUsername,
                    FirearmCount = await _context.Firearms.CountAsync(),
                    ActivityCount = await _context.Activities.CountAsync(),
                    AccessoryCount = await _context.Accessories.CountAsync(),
                    AmmunitionCount = await _context.Ammunition.CountAsync(),
                    DocumentCount = await _context.Documents.CountAsync(),
                    UserCount = await _context.Users.CountAsync()
                };

                // Add metadata.json
                var metadataEntry = archive.CreateEntry("metadata.json");
                using (var entryStream = metadataEntry.Open())
                {
                    await JsonSerializer.SerializeAsync(entryStream, metadata, new JsonSerializerOptions { WriteIndented = true });
                }

                // Configure JSON options to ignore cycles (navigation properties)
                var jsonOptions = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
                };

                // Export all tables using AsNoTracking to avoid loading navigation properties
                await ExportTableAsync(archive, "firearms.json",
                    await _context.Firearms.AsNoTracking().ToListAsync(), jsonOptions);
                await ExportTableAsync(archive, "activities.json",
                    await _context.Activities.AsNoTracking().ToListAsync(), jsonOptions);
                await ExportTableAsync(archive, "accessories.json",
                    await _context.Accessories.AsNoTracking().ToListAsync(), jsonOptions);
                await ExportTableAsync(archive, "ammunition.json",
                    await _context.Ammunition.AsNoTracking().ToListAsync(), jsonOptions);
                await ExportTableAsync(archive, "ammunition_transactions.json",
                    await _context.AmmunitionTransactions.AsNoTracking().ToListAsync(), jsonOptions);
                await ExportTableAsync(archive, "firearm_ammunition_links.json",
                    await _context.FirearmAmmunitionLinks.AsNoTracking().ToListAsync(), jsonOptions);
                await ExportTableAsync(archive, "documents.json",
                    await _context.Documents.AsNoTracking().ToListAsync(), jsonOptions);
                await ExportTableAsync(archive, "users.json",
                    await _context.Users.AsNoTracking().ToListAsync(), jsonOptions);

                // Add uploaded files
                var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads");
                if (Directory.Exists(uploadsPath))
                {
                    await AddDirectoryToArchive(archive, uploadsPath, "files/uploads/");
                }
            }
            _logger.LogInformation("Backup created successfully");
            return memoryStream.ToArray();
        }

        public async Task<BackupMetadata> ValidateBackupAsync(Stream backupStream)
        {
            _logger.LogInformation("Validating backup file");

            try
            {
                using var archive = new ZipArchive(backupStream, ZipArchiveMode.Read, true);

                // Find and read metadata
                var metadataEntry = archive.GetEntry("metadata.json");
                if (metadataEntry == null)
                {
                    throw new InvalidOperationException("Backup file is invalid: metadata.json not found");
                }

                BackupMetadata? metadata;
                using (var stream = metadataEntry.Open())
                {
                    metadata = await JsonSerializer.DeserializeAsync<BackupMetadata>(stream);
                }

                if (metadata == null)
                {
                    throw new InvalidOperationException("Backup file is invalid: metadata could not be read");
                }

                // Version check - only allow restore from same or older version
                var currentVersion = new Version("0.6.0");
                var backupVersion = new Version(metadata.Version);

                if (backupVersion != currentVersion)
                {
                    throw new InvalidOperationException(
                        $"Backup version mismatch: Backup was created with version {metadata.Version} but this application is version {AppVersion.Current}. Backups can only be restored to the same version they were created with.");
                }

                _logger.LogInformation("Backup validation successful. Version: {Version}, Created: {Date}, Files: {Count}",
                    metadata.Version, metadata.CreatedDate, archive.Entries.Count);

                return metadata;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Backup validation failed");
                throw;
            }
        }

        public async Task RestoreBackupAsync(Stream backupStream)
        {
            _logger.LogInformation("Starting database restore");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                using var archive = new ZipArchive(backupStream, ZipArchiveMode.Read, true);

                // Clear existing data
                _logger.LogInformation("Clearing existing data");
                await ClearDatabaseAsync();

                // Restore tables in correct order (respecting foreign keys)
                await RestoreTableAsync<User>(archive, "users.json");
                await RestoreTableAsync<Firearm>(archive, "firearms.json");
                await RestoreTableAsync<Activity>(archive, "activities.json");
                await RestoreTableAsync<Accessory>(archive, "accessories.json");
                await RestoreTableAsync<Ammunition>(archive, "ammunition.json");
                await RestoreTableAsync<AmmunitionTransaction>(archive, "ammunition_transactions.json");
                await RestoreTableAsync<FirearmAmmunitionLink>(archive, "firearm_ammunition_links.json");
                await RestoreTableAsync<Document>(archive, "documents.json");

                // Restore uploaded files
                var uploadsPath = Path.Combine(_environment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsPath);

                foreach (var entry in archive.Entries.Where(e => e.FullName.StartsWith("files/uploads/")))
                {
                    var relativePath = entry.FullName.Substring("files/uploads/".Length);
                    var destinationPath = Path.Combine(uploadsPath, relativePath);

                    // Create directory if needed
                    var directory = Path.GetDirectoryName(destinationPath);
                    if (!string.IsNullOrEmpty(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    // Extract file
                    using var entryStream = entry.Open();
                    using var fileStream = File.Create(destinationPath);
                    await entryStream.CopyToAsync(fileStream);
                }

                await transaction.CommitAsync();
                _logger.LogInformation("Database restore completed successfully");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Database restore failed, changes rolled back");

                // Log inner exceptions
                var innerEx = ex.InnerException;
                while (innerEx != null)
                {
                    _logger.LogError(innerEx, "Inner exception: {Message}", innerEx.Message);
                    innerEx = innerEx.InnerException;
                }

                throw;
            }
        }

        private async Task ExportTableAsync<T>(ZipArchive archive, string fileName, List<T> data, JsonSerializerOptions options)
        {
            var entry = archive.CreateEntry(fileName);
            using var stream = entry.Open();
            await JsonSerializer.SerializeAsync(stream, data, options);
        }

        private async Task AddDirectoryToArchive(ZipArchive archive, string sourceDir, string archivePath)
        {
            var files = Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var relativePath = Path.GetRelativePath(sourceDir, file);
                var entryName = archivePath + relativePath.Replace("\\", "/");

                var entry = archive.CreateEntry(entryName);
                using var entryStream = entry.Open();
                using var fileStream = File.OpenRead(file);
                await fileStream.CopyToAsync(entryStream);
            }
        }

        private async Task ClearDatabaseAsync()
        {
            // Use raw SQL to truncate all tables and reset sequences
            // This is much cleaner than EF Core's RemoveRange

            await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"Documents\" RESTART IDENTITY CASCADE");
            await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"FirearmAmmunitionLinks\" RESTART IDENTITY CASCADE");
            await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"AmmunitionTransactions\" RESTART IDENTITY CASCADE");
            await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"Ammunition\" RESTART IDENTITY CASCADE");
            await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"Accessories\" RESTART IDENTITY CASCADE");
            await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"Activities\" RESTART IDENTITY CASCADE");
            await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"Firearms\" RESTART IDENTITY CASCADE");
            await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"Users\" RESTART IDENTITY CASCADE");

            // Clear EF Core's change tracker
            _context.ChangeTracker.Clear();

            _logger.LogInformation("Database cleared and sequences reset");
        }

        private async Task RestoreTableAsync<T>(ZipArchive archive, string fileName) where T : class
        {
            var entry = archive.GetEntry(fileName);
            if (entry == null)
            {
                _logger.LogWarning("Backup file missing: {FileName}", fileName);
                return;
            }

            using var stream = entry.Open();

            // Configure JSON options to ignore navigation properties and reference loops
            var jsonOptions = new JsonSerializerOptions
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
            };

            var data = await JsonSerializer.DeserializeAsync<List<T>>(stream, jsonOptions);

            if (data != null && data.Any())
            {
                // Clear change tracker before adding
                _context.ChangeTracker.Clear();

                _context.Set<T>().AddRange(data);
                await _context.SaveChangesAsync();

                // Clear again after save to prevent tracking issues with next table
                _context.ChangeTracker.Clear();

                _logger.LogInformation("Restored {Count} records to {Table}", data.Count, typeof(T).Name);
            }
        }
    }
}