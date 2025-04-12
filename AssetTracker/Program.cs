using AssetTracker.Data;
using AssetTracker.Data.Repositories;
using AssetTracker.Services;

namespace AssetTracker;

class Program
{
    static void Main(string[] args)
    {
        // no services to support DI for now... we add it later
        var db = new AssetTrackerDbContext();
        var assetRepo = new AssetRepo(db);
        var userRepo = new UserRepo(db);
        var assetService = new AssetService(assetRepo, userRepo);
        var userService = new UserService(userRepo);

        db.EnsureDatabase();
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
            Console.WriteLine("7. Exit");
            Console.Write("Choose an option: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1": userService.AddUser(); break;
                case "2": assetService.AddAsset(); break;
                case "3": assetService.AssignAsset(); break;
                case "4": assetService.UpdateAsset(); break;
                case "5": assetService.DeleteAsset(); break;
                case "6": assetService.ViewAssets(); break;
                case "7": return;
                default:
                    Console.WriteLine("Invalid option. Press any key to try again.");
                    Console.ReadKey();
                    break;
            }
        }
    }
}