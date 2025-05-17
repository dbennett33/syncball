namespace Syncball.Core.Data.Models;

public class FixtureStats
{
    public int Id { get; set; }
    public DateTime LastUpdated { get; set; }
    public int FixtureId { get; set; }
    public Fixture Fixture { get; set; } = null!;
    public int TeamId { get; set; }
    public Team Team { get; set; } = null!;
    public int ShotsOnGoal { get; set; }
    public int ShotsOffGoal { get; set; }
    public int TotalShots { get; set; }
    public int BlockedShots { get; set; }
    public int ShotsInsideBox { get; set; }
    public int ShotsOutsideBox { get; set; }
    public int Fouls { get; set; }
    public int CornerKicks { get; set; }
    public int Offsides { get; set; }
    public int BallPossession { get; set; }
    public int YellowCards { get; set; }
    public int RedCards { get; set; }
    public int GoalkeeperSaves { get; set; }
    public int TotalPasses { get; set; }
    public int PassesAccurate { get; set; }
    public int PassesPercentage { get; set; }
}