namespace Syncball.Core.Data.Models;

public class SystemSettings
{
    public int Id { get; set; }
    public DateTime LastUpdated { get; set; }
    public string CurrentVersion { get; set; } = string.Empty;
    public bool Installed { get; set; }
}