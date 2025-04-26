using AssetTracker.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AssetTracker.Data;

/// <summary>
/// Create a database schama if it doenst exist
/// </summary>
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


    //public SQLiteConnection GetConnection()
    //{
    //    return new SQLiteConnection(_connectionString);
    //}

    //public void EnsureDatabase()
    //{
    //    using var conn = new SQLiteConnection(_connectionString);
    //    conn.Open();

    //    string createUsers = @"CREATE TABLE IF NOT EXISTS Users (
    //            Id INTEGER PRIMARY KEY AUTOINCREMENT,
    //            Name TEXT NOT NULL
    //        );";

    //    string createAssets = @"CREATE TABLE IF NOT EXISTS Assets (
    //            Id INTEGER PRIMARY KEY AUTOINCREMENT,
    //            Name TEXT NOT NULL,
    //            Type TEXT,
    //            SerialNumber TEXT,
    //            AssignedUserId INTEGER,
    //            FOREIGN KEY (AssignedUserId) REFERENCES Users(Id)
    //        );";

    //    using var cmd1 = new SQLiteCommand(createUsers, conn);
    //    using var cmd2 = new SQLiteCommand(createAssets, conn);
    //    cmd1.ExecuteNonQuery();
    //    cmd2.ExecuteNonQuery();
    //}
}