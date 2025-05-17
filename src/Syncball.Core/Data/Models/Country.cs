namespace Syncball.Core.Data.Models;

public class Country
{
    public int Id { get; set; }
    public DateTime LastUpdated { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string FlagUrl { get; set; } = string.Empty;
    public bool Enabled { get; set; }
    public ICollection<League> Leagues { get; set; } = null!;
}