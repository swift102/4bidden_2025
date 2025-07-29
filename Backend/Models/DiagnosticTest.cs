namespace Backend.Models
{
    public class DiagnosticTest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public int UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
    }
}
