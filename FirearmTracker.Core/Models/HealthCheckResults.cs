public class HealthCheckResults
{
    public bool FfmpegAvailable { get; set; }

    public bool HasWarnings => !FfmpegAvailable;

    public List<string> GetWarningMessages()
    {
        var warnings = new List<string>();

        if (!FfmpegAvailable)
        {
            warnings.Add("FFMPEG is not installed or not found in PATH. Video thumbnails will not be generated.");
        }

        return warnings;
    }
}