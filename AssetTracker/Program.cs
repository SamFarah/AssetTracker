using AssetTracker.Backend.Data;
using AssetTracker.Backend.Data.Repositories;
using AssetTracker.Backend.Services;
using AssetTracker.Controllers;
using Microsoft.EntityFrameworkCore;

namespace AssetTracker;

class Program
{
    static async Task Main(string[] args)
    {
        var cs = "Data Source=AssetTracker.db";

        // no services to support DI for now... we add it later
        var dbOptions = new DbContextOptionsBuilder<AssetTrackerDbContext>().UseSqlite(cs).Options;
        var db = new AssetTrackerDbContext(dbOptions);
        var assetRepo = new AssetRepo(db);
        var userRepo = new UserRepo(db);


        // this is why DI is important.
        var menuController = new MenuController(new UserController(new UserService(userRepo)), new AssetsController(new AssetService(assetRepo, userRepo), new UserService(userRepo)));

        //db.Database.Migrate();

        await menuController.Run();
    }
}