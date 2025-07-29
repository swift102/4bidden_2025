using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class UpdateProfileDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public bool NotificationsEnabled { get; set; }

        [Required]
        public string Theme { get; set; } = "light";

        [Range(1, 100)]
        public int NotificationThreshold { get; set; } = 5;

        public string TimeZone { get; set; } = "UTC";
        public bool EmailNotifications { get; set; }
        public bool SmsNotifications { get; set; }
    }
}
