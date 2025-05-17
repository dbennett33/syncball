namespace Syncball.Core.Data.Models;

public class Venue
{
    public int Id { get; set; }
    public DateTime LastUpdated { get; set ; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public string Surface { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
}