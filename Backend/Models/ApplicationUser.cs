using Microsoft.AspNetCore.Identity;

namespace Backend.Models
{
    public class ApplicationUser : IdentityUser
    {
      
        public string Name { get; set; } = string.Empty;
        

        // User Preferences
        public bool NotificationsEnabled { get; set; } = true;
        public string Theme { get; set; } = "light"; // light, dark
        public int NotificationThreshold { get; set; } = 5; // Number of alerts before notification
        public string TimeZone { get; set; } = "UTC";
        public bool EmailNotifications { get; set; } = true;
        public bool SmsNotifications { get; set; } = false;

        // Timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
 
    }
}
