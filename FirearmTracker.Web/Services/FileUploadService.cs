using Docnet.Core;
using Docnet.Core.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using FFMpegCore;

namespace FirearmTracker.Web.Services
{
    public class FileUploadService
    {
        private readonly string _uploadPath;
        private readonly string _thumbnailPath;
        private const long MaxFileSize = 50 * 1024 * 1024; // 50MB
        private readonly string[] AllowedExtensions = { ".pdf", ".jpg", ".jpeg", ".png", ".doc", ".docx", ".mp4", ".mov", ".avi", ".wmv", ".mkv" };

        public FileUploadService(IWebHostEnvironment environment)
        {
            _uploadPath = Path.Combine(environment.WebRootPath, "uploads");
            _thumbnailPath = Path.Combine(environment.WebRootPath, "uploads", "thumbnails");

            // Ensure upload directories exist
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
            if (!Directory.Exists(_thumbnailPath))
            {
                Directory.CreateDirectory(_thumbnailPath);
            }
        }

        public async Task<(bool success, string? fileName, string? thumbnailFileName, string? errorMessage)> SaveFileAsync(
            Stream fileStream,
            string originalFileName,
            string contentType)
        {
            try
            {
                // Validate file extension
                var extension = Path.GetExtension(originalFileName).ToLowerInvariant();
                if (!AllowedExtensions.Contains(extension))
                {
                    return (false, null, null, "File type not allowed. Allowed types: PDF, JPG, PNG, DOC, DOCX, MP4, MOV, AVI, WMV, MKV");
                }

                // Validate file size
                if (fileStream.Length > MaxFileSize)
                {
                    return (false, null, null, "File size exceeds 50MB limit");
                }

                // Generate unique filename
                var uniqueFileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(_uploadPath, uniqueFileName);

                // Save file
                using (var fileStreamOutput = new FileStream(filePath, FileMode.Create))
                {
                    await fileStream.CopyToAsync(fileStreamOutput);
                }

                // Generate thumbnail if applicable (will be implemented for PDF and video)
                string? thumbnailFileName = null;

                // For now, return null for thumbnail - we'll implement generation in next steps
                if (contentType == "application/pdf")
                {
                    thumbnailFileName = await GeneratePdfThumbnailAsync(filePath);
                }
                else if (contentType.StartsWith("video/"))
                {
                    thumbnailFileName = await GenerateVideoThumbnailAsync(filePath);
                }

                return (true, uniqueFileName, thumbnailFileName, null);
            }
            catch (Exception ex)
            {
                return (false, null, null, $"Error saving file: {ex.Message}");
            }
        }

        public string GetFilePath(string fileName)
        {
            return Path.Combine(_uploadPath, fileName);
        }

        public string GetThumbnailPath(string thumbnailFileName)
        {
            return Path.Combine(_thumbnailPath, thumbnailFileName);
        }

        public bool DeleteFile(string fileName)
        {
            try
            {
                var filePath = Path.Combine(_uploadPath, fileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteThumbnail(string? thumbnailFileName)
        {
            if (string.IsNullOrEmpty(thumbnailFileName))
                return false;

            try
            {
                var thumbnailPath = Path.Combine(_thumbnailPath, thumbnailFileName);
                if (File.Exists(thumbnailPath))
                {
                    File.Delete(thumbnailPath);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private async Task<string?> GeneratePdfThumbnailAsync(string filePath)
        {
            try
            {
                using (var docReader = DocLib.Instance.GetDocReader(filePath, new PageDimensions(1024, 1024)))
                {
                    if (docReader.GetPageCount() == 0)
                        return null;

                    using (var pageReader = docReader.GetPageReader(0)) // First page
                    {
                        var rawBytes = pageReader.GetImage();
                        var width = pageReader.GetPageWidth();
                        var height = pageReader.GetPageHeight();

                        // Generate unique thumbnail filename
                        var thumbnailFileName = $"{Guid.NewGuid()}.png";
                        var thumbnailPath = Path.Combine(_thumbnailPath, thumbnailFileName);

                        // Save as PNG
                        using (var image = Image.LoadPixelData<Bgra32>(rawBytes, width, height))
                        {
                            // Resize to max 256x256 while maintaining aspect ratio
                            image.Mutate(x => x.Resize(new ResizeOptions
                            {
                                Size = new Size(256, 256),
                                Mode = ResizeMode.Max
                            }));

                            await image.SaveAsPngAsync(thumbnailPath);
                        }

                        return thumbnailFileName;
                    }
                }
            }
            catch (Exception)
            {
                return null; // If thumbnail generation fails, just return null
            }
        }

        private async Task<string?> GenerateVideoThumbnailAsync(string filePath)
        {
            try
            {
                var thumbnailFileName = $"{Guid.NewGuid()}.png";
                var thumbnailPath = Path.Combine(_thumbnailPath, thumbnailFileName);

                // Extract frame at 1 second into the video
                var success = await FFMpeg.SnapshotAsync(filePath, thumbnailPath, new System.Drawing.Size(256, 256), TimeSpan.FromSeconds(1));

                return success ? thumbnailFileName : null;
            }
            catch (Exception)
            {
                return null; // If thumbnail generation fails, just return null
            }
        }
    }
}