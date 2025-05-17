namespace Syncball.EF;

using Microsoft.EntityFrameworkCore;
using Syncball.Core.Data.Configurations;
using Syncball.Core.Data.Models;

public class SyncballContext : DbContext
{
    public DbSet<SystemSettings> SystemSettings { get; set; }
    public DbSet<InstallInfo> InstallInfo { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<League> Leagues { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Venue> Venues { get; set; }
    public DbSet<Fixture> Fixtures { get; set; }
    public DbSet<Season> Seasons { get; set; }
    public DbSet<Coverage> Coverage { get; set; }
    public DbSet<FixtureStats> FixtureStats { get; set; }

    public SyncballContext(DbContextOptions<SyncballContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CountryConfiguration());
        modelBuilder.ApplyConfiguration(new LeagueConfiguration());
        modelBuilder.ApplyConfiguration(new TeamConfiguration());
        modelBuilder.ApplyConfiguration(new VenueConfiguration());
        modelBuilder.ApplyConfiguration(new FixtureConfiguration());
        modelBuilder.ApplyConfiguration(new SeasonConfiguration());
        modelBuilder.ApplyConfiguration(new CoverageConfiguration());
        modelBuilder.ApplyConfiguration(new FixtureStatsConfiguration());
        modelBuilder.ApplyConfiguration(new SystemSettingsConfiguration());
        modelBuilder.ApplyConfiguration(new InstallInfoConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}