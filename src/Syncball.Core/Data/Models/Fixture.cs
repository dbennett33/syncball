namespace Syncball.Core.Data.Models;

public class Fixture 
{
    public int Id { get; set; }
    public DateTime LastUpdated { get; set; }
    public string Referee { get; set; } = string.Empty;
    public string Timezone { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public string Timestamp { get; set; } = string.Empty;
    public int VenueId { get; set; }
    public Venue Venue { get; set; } = null!;
    public string Status { get; set; } = string.Empty;
    public int TimeElapsed { get; set; }
    public int LeagueId { get; set; }
    public League League { get; set; } = null!;
    public int SeasonId { get; set; }
    public Season Season { get; set; } = null!;
    public string Round { get; set; } = string.Empty;
    public int HomeTeamId { get; set; }
    public Team HomeTeam { get; set; } = null!;
    public int AwayTeamId { get; set; }
    public Team AwayTeam { get; set; } = null!;
    public int GoalsHomeTeam { get; set; }
    public int GoalsAwayTeam { get; set; }
    public int GoalsHomeTeamHT { get; set; }
    public int GoalsAwayTeamHT { get; set; }
    public int GoalsHomeTeamFT { get; set; }
    public int GoalsAwayTeamFT { get; set; }
    public int GoalsHomeTeamET { get; set; }
    public int GoalsAwayTeamET { get; set; }
    public int GoalsHomeTeamPen { get; set; }
    public int GoalsAwayTeamPen { get; set;}
    public int FixtureStatsId { get; set; }
    public ICollection<FixtureStats> Stats { get; set; } = null!;


}