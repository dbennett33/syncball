namespace Syncball.Core.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Syncball.Core.Data.Models;

public class SeasonConfiguration : IEntityTypeConfiguration<Season>
{
    public void Configure(EntityTypeBuilder<Season> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Year).IsRequired().HasMaxLength(9);
        builder.Property(s => s.StartDate).IsRequired();
        builder.Property(s => s.EndDate).IsRequired();
        builder.Property(s => s.Current).IsRequired();

        builder.HasOne(s => s.League)
               .WithMany(l => l.Seasons)
               .HasForeignKey(s => s.LeagueId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Coverage)
               .WithOne(c => c.Season)
               .HasForeignKey<Coverage>(c => c.SeasonId)
               .OnDelete(DeleteBehavior.Cascade);

    }
}