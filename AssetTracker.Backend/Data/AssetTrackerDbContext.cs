using AssetTracker.Backend.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AssetTracker.Backend.Data;

public class AssetTrackerDbContext(DbContextOptions<AssetTrackerDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Asset> Assets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseLazyLoadingProxies();
    }
}