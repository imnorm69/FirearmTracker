using FirearmTracker.Core.Interfaces;
using FirearmTracker.Core.Models;

namespace FirearmTracker.Web.Services
{
    public class HealthCheckService(ILogger<HealthCheckService> logger) : IHealthCheckService
    {
        private readonly ILogger<HealthCheckService> _logger = logger;
        private HealthCheckResults? _cachedResults;
        private DateTime _lastCheckTime = DateTime.MinValue;
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(5);

        public async Task<HealthCheckResults> RunChecksAsync()
        {
            // Return cached results if still valid
            if (_cachedResults != null && DateTime.Now - _lastCheckTime < _cacheExpiration)
            {
                return _cachedResults;
            }

            var results = new HealthCheckResults
            {
                // Check FFMPEG
                FfmpegAvailable = await CheckFfmpegAsync()
            };

            _cachedResults = results;
            _lastCheckTime = DateTime.Now;

            return results;
        }

        private async Task<bool> CheckFfmpegAsync()
        {
            try
            {
                var processStartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "ffmpeg",
                    Arguments = "-version",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = System.Diagnostics.Process.Start(processStartInfo);
                if (process == null)
                {
                    _logger.LogWarning("FFMPEG check failed: Could not start process");
                    return false;
                }

                // Log the full path
                try
                {
                    var fullPath = process.MainModule?.FileName;
                    if (!string.IsNullOrEmpty(fullPath))
                    {
                        _logger.LogInformation("FFMPEG found at: {Path}", fullPath);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogDebug(ex, "Could not determine FFMPEG path");
                }

                await process.WaitForExitAsync();
                var isAvailable = process.ExitCode == 0;

                if (!isAvailable)
                {
                    _logger.LogWarning("FFMPEG check failed: Exit code {ExitCode}", process.ExitCode);
                }
                else
                {
                    _logger.LogInformation("FFMPEG is available and working");
                }

                return isAvailable;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "FFMPEG check failed with exception");
                return false;
            }
        }

        //private async Task<bool> CheckFfmpegAsync()
        //{
        //    try
        //    {
        //        var processStartInfo = new System.Diagnostics.ProcessStartInfo
        //        {
        //            FileName = "ffmpeg",
        //            Arguments = "-version",
        //            RedirectStandardOutput = true,
        //            RedirectStandardError = true,
        //            UseShellExecute = false,
        //            CreateNoWindow = true
        //        };

        //        using var process = System.Diagnostics.Process.Start(processStartInfo);
        //        if (process == null)
        //        {
        //            _logger.LogWarning("FFMPEG check failed: Could not start process");
        //            return false;
        //        }

        //        await process.WaitForExitAsync();
        //        var isAvailable = process.ExitCode == 0;

        //        if (!isAvailable)
        //        {
        //            _logger.LogWarning("FFMPEG check failed: Exit code {ExitCode}", process.ExitCode);
        //        }
        //        else
        //        {
        //            _logger.LogInformation("FFMPEG is available");
        //        }

        //        return isAvailable;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogWarning(ex, "FFMPEG check failed with exception");
        //        return false;
        //    }
        //}
    }
}