namespace Syncball.Core.Data.Models;

public class InstallInfo 
{
    public int Id { get; set; }
    public DateTime LastUpdated { get; set; }
    public int SystemSettingsId { get; set; }
    public SystemSettings SystemSettings { get; set; } = null!;
    public int Version { get; set; }
    public DateTime InstallStart { get; set; }
    public DateTime InstallEnd { get; set; }
    public bool Complete { get; set; }
    public bool EnabledEntitiesApplied { get; set; }
    public string EnabledEntitiesJson { get; set; } = string.Empty;
    public bool CountriesInstalled { get; set; }
    public bool LeaguesInstalled { get; set; }
    public bool TeamsInstalled { get; set; }
    public bool FixturesInstalled { get; set; }
}