namespace Syncball.Core.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Syncball.Core.Data.Models;

public class FixtureConfiguration : IEntityTypeConfiguration<Fixture>
{
    public void Configure(EntityTypeBuilder<Fixture> builder)
    {
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Id).ValueGeneratedNever();

        builder.Property(f => f.Referee).HasMaxLength(100);
        builder.Property(f => f.Timezone).IsRequired().HasMaxLength(100);
        builder.Property(f => f.StartTime).IsRequired();
        builder.Property(f => f.Timestamp).IsRequired().HasMaxLength(100);
        builder.Property(f => f.Status).HasMaxLength(100);
        builder.Property(f => f.TimeElapsed).IsRequired();
        builder.Property(f => f.Round).HasMaxLength(50);

        builder.HasOne(f => f.League)
            .WithMany()
            .HasForeignKey(f => f.LeagueId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.Season)
            .WithMany()
            .HasForeignKey(f => f.SeasonId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.HomeTeam)
            .WithMany()
            .HasForeignKey(f => f.HomeTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.AwayTeam)
            .WithMany()
            .HasForeignKey(f => f.AwayTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(f => f.Stats)
               .WithOne()
               .HasForeignKey(fs => fs.FixtureId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}