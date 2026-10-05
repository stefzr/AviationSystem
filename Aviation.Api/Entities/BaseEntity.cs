namespace Aviation.Api.Entities;

public abstract class BaseEntity
{
    // Το Primary Key πλέον ορίζεται ρητά ως long
    public long Id { get; set; }
    
    public DateTime CreatedOn { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    
    public DateTime LastUpdatedOn { get; set; }
    public string LastUpdatedBy { get; set; } = string.Empty;
}