using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.DataAccess.Configurations;

public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder.ToTable("Recipes");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).IsRequired().HasMaxLength(100);
        builder.Property(x => x.CookingTime);
        builder.Property(x => x.Difficulty);

        builder.HasOne(u => u.User)
            .WithMany(u => u.Recipes)
            .HasForeignKey(r => r.UserId);
    }
}