using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infra.DataAccess;

public class CookBookDbContext: DbContext
{
    public CookBookDbContext(DbContextOptions options): base(options) { }
    
    // Tables
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CookBookDbContext).Assembly);
    }
}