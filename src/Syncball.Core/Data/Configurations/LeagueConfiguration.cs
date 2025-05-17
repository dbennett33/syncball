namespace Syncball.Core.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Syncball.Core.Data.Models;

public class LeagueConfiguration : IEntityTypeConfiguration<League>
{
    public void Configure(EntityTypeBuilder<League> builder)
    {
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id).ValueGeneratedNever();

        // Configure properties
        builder.Property(l => l.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(l => l.Type)
            .HasMaxLength(50);

        builder.Property(l => l.LogoUrl)
            .HasMaxLength(200);

        builder.HasOne(l => l.Country)
            .WithMany(c => c.Leagues)
            .HasForeignKey(l => l.CountryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure the relationship with Seasons
        builder.HasMany(l => l.Seasons)
            .WithOne(s => s.League)
            .HasForeignKey(s => s.LeagueId)
            .OnDelete(DeleteBehavior.Restrict); // Optional: configure cascading delete behavior

    }
}