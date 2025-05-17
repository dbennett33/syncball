namespace Syncball.Core.Data.Models;

public class Coverage 
{
    public int Id { get; set; }
    public DateTime LastUpdated { get; set; }
    public int SeasonId { get; set; }
    public Season Season { get; set; } = null!;
    public bool Events { get; set; }
    public bool Lineups { get; set; }
    public bool FixtureStats { get; set; }
    public bool PlayerStats { get; set; }
    public bool Standings { get; set; }
    public bool Players { get; set; }
    public bool TopScorers { get; set; }
    public bool TopAssists { get; set; }
    public bool TopCards { get; set; }
    public bool Injuries { get; set; }
    public bool Predictions { get; set; }
    public bool Odds { get; set; }   
}