namespace Backend.Models
{
    public class MonitoredDestination
    {
        public int Id { get; set; }
        public string Location { get; set; } = string.Empty;
        public string RiskLevel { get; set; } = string.Empty;
        public DateTime LastChecked { get; set; }
        public string UserId { get; set; } = string.Empty;

    }
}
