namespace Syncball.Core.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Syncball.Core.Data.Models;

public class FixtureStatsConfiguration : IEntityTypeConfiguration<FixtureStats>
{
    public void Configure(EntityTypeBuilder<FixtureStats> builder)
    {
        builder.HasKey(fs => fs.Id);

        builder.HasOne(fs => fs.Fixture)
               .WithMany(f => f.Stats)
               .HasForeignKey(fs => fs.FixtureId)
               .OnDelete(DeleteBehavior.Restrict);

        // Configure relationship with Team entity
        builder.HasOne(fs => fs.Team)
               .WithMany()
               .HasForeignKey(fs => fs.TeamId)
               .OnDelete(DeleteBehavior.Restrict);

        // Configure properties
        builder.Property(fs => fs.ShotsOnGoal).IsRequired();
        builder.Property(fs => fs.ShotsOffGoal).IsRequired();
        builder.Property(fs => fs.TotalShots).IsRequired();
        builder.Property(fs => fs.BlockedShots).IsRequired();
        builder.Property(fs => fs.ShotsInsideBox).IsRequired();
        builder.Property(fs => fs.ShotsOutsideBox).IsRequired();
        builder.Property(fs => fs.Fouls).IsRequired();
        builder.Property(fs => fs.CornerKicks).IsRequired();
        builder.Property(fs => fs.Offsides).IsRequired();
        builder.Property(fs => fs.BallPossession).IsRequired();
        builder.Property(fs => fs.YellowCards).IsRequired();
        builder.Property(fs => fs.RedCards).IsRequired();
        builder.Property(fs => fs.GoalkeeperSaves).IsRequired();
        builder.Property(fs => fs.TotalPasses).IsRequired();
        builder.Property(fs => fs.PassesAccurate).IsRequired();
        builder.Property(fs => fs.PassesPercentage).IsRequired();
    }
}