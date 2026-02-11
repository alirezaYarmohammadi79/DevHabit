using DevHabitApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevHabitApi.Database.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(h => h.Id).HasMaxLength(500);
        
        builder.Property(h => h.Email).HasMaxLength(300);
        builder.Property(h => h.Name).HasMaxLength(100);
        builder.Property(h => h.IdentityId).HasMaxLength(500);

        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.IdentityId).IsUnique();
    }
}
