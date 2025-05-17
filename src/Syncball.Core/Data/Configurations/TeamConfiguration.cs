namespace Syncball.Core.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Syncball.Core.Data.Models;

public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedNever();

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.Code)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(t => t.Country)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(t => t.Founded)
            .HasMaxLength(4);

        builder.Property(t => t.NationalTeam)
            .IsRequired();

        builder.Property(t => t.LogoUrl)
            .HasMaxLength(200);
    }
}