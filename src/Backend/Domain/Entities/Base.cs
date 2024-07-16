namespace Domain.Entities;

public class Base
{
    public long Id { get; set; }
    public bool Active { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}