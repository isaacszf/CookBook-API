using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.DataAccess.Configurations;

public class InstructionConfiguration : IEntityTypeConfiguration<Instruction>
{
    public void Configure(EntityTypeBuilder<Instruction> builder)
    {
        builder.ToTable("Instructions");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Step).IsRequired();
        builder.Property(x => x.Description).IsRequired().HasMaxLength(2000);

        builder.HasOne(i => i.Recipe)
            .WithMany(r => r.Instructions)
            .HasForeignKey(i => i.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}