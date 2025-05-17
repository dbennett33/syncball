namespace Syncball.Core.Data.Models;

public class League 
{
    public int Id { get; set; }
    public DateTime LastUpdated { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string LogoUrl { get; set; } = string.Empty;
    public bool Enabled { get; set; }
    public int CountryId { get; set; }
   public Country Country { get; set; } = null!;
   public ICollection<Season> Seasons { get; set; } = [];
}