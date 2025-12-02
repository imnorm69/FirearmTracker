using System.ComponentModel.DataAnnotations;

namespace FirearmTracker.Core.Models
{
    public class Document
    {
        public int Id { get; set; }

        public int? FirearmId { get; set; }
        public Firearm? Firearm { get; set; }

        public int? ActivityId { get; set; }
        public Activity? Activity { get; set; }

        [Required]
        [MaxLength(255)]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string OriginalFileName { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? ThumbnailFileName { get; set; }

        [Required]
        [MaxLength(100)]
        public string ContentType { get; set; } = string.Empty;

        public long FileSize { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public DateTime UploadedDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}