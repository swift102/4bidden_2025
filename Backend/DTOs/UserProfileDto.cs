namespace Backend.DTOs
{
    public class UserProfileDto
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public bool NotificationsEnabled { get; set; }
        public string Theme { get; set; } = string.Empty;
        public int NotificationThreshold { get; set; }
        public string TimeZone { get; set; } = string.Empty;
        public bool EmailNotifications { get; set; }
        public bool SmsNotifications { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
