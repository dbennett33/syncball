namespace Syncball.Core.Data.Models;

public class Season
{
    public int Id { get; set; }
    public DateTime LastUpdated { get; set; }
    public int LeagueId { get; set; }
    public League League { get; set; } = null!;
    public string Year { get; set; } = string.Empty;
    public string StartDate { get; set; } = string.Empty;
    public string EndDate { get; set; } = string.Empty;
    public bool Current { get; set; }
    public int CoverageId { get; set; }
    public Coverage Coverage { get; set; } = null!;
}