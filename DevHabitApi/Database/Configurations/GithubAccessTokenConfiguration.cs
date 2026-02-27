using DevHabitApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevHabitApi.Database.Configurations;

public class GithubAccessTokenConfiguration : IEntityTypeConfiguration<GithubAccessToken>
{
    public void Configure(EntityTypeBuilder<GithubAccessToken> builder)
    {
        builder.HasKey(g => g.Id);

        builder.Property(gh => gh.Id).HasMaxLength(500);
        builder.Property(gh => gh.UserId).HasMaxLength(500);
        builder.Property(gh => gh.Token).HasMaxLength(1000);

        builder.HasIndex(gh => gh.UserId).IsUnique();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(gh => gh.UserId);
    }
}
