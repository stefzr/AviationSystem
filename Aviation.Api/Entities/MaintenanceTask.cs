namespace Aviation.Api.Entities;

// Κληρονομεί από το BaseEntity
public class MaintenanceTask : BaseEntity
{
    // Το Id ΔΙΑΓΡΑΦΗΚΕ από εδώ γιατί υπάρχει πλέον στο BaseEntity
    public string AircraftTailNumber { get; set; } = string.Empty;
    public string AircraftRegistration { get; set; } = string.Empty; // Π.χ. "SX-DZA"
    public string Component { get; set; } = string.Empty;             // Π.χ. "Landing Gear", "Engine #1"
    public string Description { get; set; } = string.Empty;
    public long StatusId { get; set; }
    public Status Status { get; set; } = null!;
    public long PriorityId { get; set; }
    public Priority Priority { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
} 

        