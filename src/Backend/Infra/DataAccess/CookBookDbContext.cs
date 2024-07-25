using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infra.DataAccess;

public class CookBookDbContext: DbContext
{
    public CookBookDbContext(DbContextOptions options): base(options) { }
    
    // Tables
    public DbSet<User> Users { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Instruction> Instructions { get; set; }
    public DbSet<DishType> DishTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CookBookDbContext).Assembly);
    }
}