namespace Aviation.Api.Models
{
    public class MaintenanceTask
    {
        public int Id { get; set; }
        public string AircraftRegistration { get; set; } = string.Empty; // Π.χ. "SX-DZA"
        public string Component { get; set; } = string.Empty;             // Π.χ. "Landing Gear", "Engine #1"
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = "PENDING";                   // PENDING, IN_PROGRESS, COMPLETED
        public string Priority { get; set; } = "NORMAL";                  // LOW, NORMAL, HIGH, AOG (Aircraft on Ground)
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}