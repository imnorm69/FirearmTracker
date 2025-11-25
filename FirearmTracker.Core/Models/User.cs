namespace FirearmTracker.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "Owner"; // Admin, Owner, Visitor
        public DateTime CreatedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public bool IsActive { get; set; } = true;

        // Avatar data stored as binary
        public byte[]? AvatarImage { get; set; }

        // MIME type to know how to serve it (e.g., "image/png" or "image/jpeg")
        public string? AvatarContentType { get; set; }
    }
}