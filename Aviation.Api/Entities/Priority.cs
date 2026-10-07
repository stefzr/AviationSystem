namespace Aviation.Api.Entities;

public class Priority : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int Severity { get; set; }
}