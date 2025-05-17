namespace Syncball.Core.Data.Configurations;

using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Syncball.Core.Data.Models;

public class SystemSettingsConfiguration : IEntityTypeConfiguration<SystemSettings>
{
    public void Configure(EntityTypeBuilder<SystemSettings> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.CurrentVersion)
            .IsRequired()
            .HasMaxLength(100);
    }
}