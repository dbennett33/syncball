namespace Syncball.Core.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Syncball.Core.Data.Models;

public class VenueConfiguration : IEntityTypeConfiguration<Venue>
{
    public void Configure(EntityTypeBuilder<Venue> builder)
    {
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id).ValueGeneratedNever();

        // Configure properties
        builder.Property(v => v.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(v => v.Address)
            .HasMaxLength(200);

        builder.Property(v => v.City)
            .HasMaxLength(100);

        builder.Property(v => v.Capacity)
            .HasMaxLength(50);

        builder.Property(v => v.Surface)
            .HasMaxLength(50);

        builder.Property(v => v.ImageUrl)
            .HasMaxLength(200);

    }
}