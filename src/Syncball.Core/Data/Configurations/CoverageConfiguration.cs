namespace Syncball.Core.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Syncball.Core.Data.Models;

public class CoverageConfiguration : IEntityTypeConfiguration<Coverage>
{
    public void Configure(EntityTypeBuilder<Coverage> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.SeasonId)
            .IsRequired();

        builder.Property(c => c.Events)
            .IsRequired();

        builder.Property(c => c.Lineups)
            .IsRequired();

        builder.Property(c => c.FixtureStats)
            .IsRequired();

        builder.Property(c => c.PlayerStats)
            .IsRequired();

        builder.Property(c => c.Standings)
            .IsRequired();

        builder.Property(c => c.Players)
            .IsRequired();

        builder.Property(c => c.TopScorers)
            .IsRequired();

        builder.Property(c => c.TopAssists)
            .IsRequired();

        builder.Property(c => c.TopCards)
            .IsRequired();

        builder.Property(c => c.Injuries)
            .IsRequired();

        builder.Property(c => c.Predictions)
            .IsRequired();

        builder.Property(c => c.Odds)
            .IsRequired();

        builder.HasOne(c => c.Season)
            .WithOne(s => s.Coverage)
            .HasForeignKey<Coverage>(c => c.SeasonId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}