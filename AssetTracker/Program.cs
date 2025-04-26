using AssetTracker.Data;
using AssetTracker.Data.Repositories;
using AssetTracker.Services;
using Microsoft.EntityFrameworkCore;

namespace AssetTracker;

class Program
{
    static async Task Main(string[] args)
    {

        var cs = "Data Source=AssetTracker.db";

        // no services to support DI for now... we add it later
        var dbOptions = new DbContextOptionsBuilder<AssetTrackerDbContext>()
            .UseSqlite(cs).Options;
        var db = new AssetTrackerDbContext(dbOptions);
        var assetRepo = new AssetRepo(db);
        var userRepo = new UserRepo(db);
        var assetService = new AssetService(assetRepo, userRepo);
        var userService = new UserService(userRepo);

        //db.Database.Migrate();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Assetrix - Asset Management ===");
            Console.WriteLine("1. Add User");
            Console.WriteLine("2. Add Asset");
            Console.WriteLine("3. Assign Asset to User");
            Console.WriteLine("4. Update Asset");
            Console.WriteLine("5. Delete Asset");
            Console.WriteLine("6. View All Assets");
            Console.WriteLine("7. View All Users");
            Console.WriteLine("8. Exit");
            Console.Write("Choose an option: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1": await userService.AddUserAsync(); break;
                case "2": await assetService.AddAssetAsync(); break;
                case "3": await assetService.AssignAssetAsync(); break;
                case "4": await assetService.UpdateAssetAsync(); break;
                case "5": await assetService.DeleteAssetAsync(); break;
                case "6": await assetService.ViewAssetsAsync(); break;
                case "7": await userService.ViewUsersAsync(); break;
                case "8": return;
                default:
                    Console.WriteLine("Invalid option. Press any key to try again.");
                    Console.ReadKey();
                    break;
            }
        }
    }
}