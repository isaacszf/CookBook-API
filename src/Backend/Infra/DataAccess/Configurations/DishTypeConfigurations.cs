using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.DataAccess.Configurations;

public class DishTypeConfigurations : IEntityTypeConfiguration<DishType>
{
    public void Configure(EntityTypeBuilder<DishType> builder)
    {
        builder.ToTable("DishTypes");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Type).IsRequired();

        builder.HasOne(i => i.Recipe)
            .WithMany(r => r.DishTypes)
            .HasForeignKey(i => i.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}