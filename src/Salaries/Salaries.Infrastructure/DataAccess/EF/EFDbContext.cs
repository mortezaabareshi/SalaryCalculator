using Microsoft.EntityFrameworkCore;
using Salaries.Core.Entities;

namespace Salaries.Infrastructure.DataAccess.EF;

public class EFDbContext : DbContext
{
    public DbSet<Salary> Salaries { get; set; }

    public EFDbContext()
    {
    }

    public EFDbContext(DbContextOptions<EFDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("salaries");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}