namespace Contracts.DatabaseModels;

public class Site
{
    public long Id { get; set; } // Primary key
    public DateTimeOffset CreatedAt { get; set; }
}