using TechChallenge.Purchases.Core.Entity;
using Microsoft.EntityFrameworkCore;

namespace TechChallenge.Purchases.Infrastructure.Repository;

public class ApplicationDbContext : DbContext
{
    private string? _connectionString;

    public ApplicationDbContext() : base() { }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }    
    
    public ApplicationDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public DbSet<Compra> Usuario { get; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlServer(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationException).Assembly);
}