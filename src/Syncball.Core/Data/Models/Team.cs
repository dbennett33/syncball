namespace Syncball.Core.Data.Models;

public class Team
{
    public int Id { get; set; }
    public DateTime LastUpdated { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Founded { get; set; } = string.Empty;
    public bool NationalTeam { get; set; }
    public string LogoUrl { get; set; } = string.Empty;
}