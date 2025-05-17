namespace Syncball.Worker.Client.ResponseModels;

public class LeagueResponse
{
    public LeagueData? League { get; set; }
    public CountryResponse? Country { get; set; }
    public List<SeasonResponse>? Seasons { get; set; }
}

public class LeagueData
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? Logo { get; set; }
}

public class SeasonResponse
{
    public int Year { get; set; }
    public string? Start { get; set; }
    public string? End { get; set; }
    public bool Current { get; set; }
    public CoverageResponse? Coverage { get; set; }
}

public class CoverageResponse
{
    public CoverageFixturesResponse? Fixtures { get; set; }
    public bool Standings { get; set; }
    public bool Players { get; set; }
    public bool TopScorers { get; set; }
    public bool TopAssists { get; set; }
    public bool TopCards { get; set; }
    public bool Injuries { get; set; }
    public bool Predictions { get; set; }
    public bool Odds { get; set; }
}

public class CoverageFixturesResponse
{
    public bool Events { get; set; }
    public bool Lineups { get; set; }
    public bool StatisticsFixtures { get; set; }
    public bool StatisticsPlayers { get; set; }
}
