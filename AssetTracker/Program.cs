using System;
using System.Collections.Generic;
using System.Linq;

namespace AssetTracker;

class Program
{
    // Models
    class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    class Asset
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string SerialNumber { get; set; }
        public int? AssignedUserId { get; set; }
    }

    // Data Stores
    static List<User> users = new List<User>();
    static List<Asset> assets = new List<Asset>();
    static int userIdCounter = 1;
    static int assetIdCounter = 1;

    static void Main(string[] args)
    {
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
                case "1": AddUser(); break;
                case "2": AddAsset(); break;
                case "3": AssignAsset(); break;
                case "4": UpdateAsset(); break;
                case "5": DeleteAsset(); break;
                case "6": ViewAssets(); break;
                case "7": return;
                default:
                    Console.WriteLine("Invalid option. Press any key to try again.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void AddUser()
    {
        Console.Clear();
        Console.WriteLine("=== Add New User ===");
        Console.Write("User Name: ");
        string name = Console.ReadLine();

        users.Add(new User
        {
            Id = userIdCounter++,
            Name = name
        });

        Console.WriteLine("User added. Press any key to return to menu.");
        Console.ReadKey();
    }

    static void AddAsset()
    {
        Console.Clear();
        Console.WriteLine("=== Add New Asset ===");

        Console.Write("Asset Name: ");
        string name = Console.ReadLine();

        Console.Write("Asset Type: ");
        string type = Console.ReadLine();

        Console.Write("Serial Number: ");
        string serial = Console.ReadLine();

        assets.Add(new Asset
        {
            Id = assetIdCounter++,
            Name = name,
            Type = type,
            SerialNumber = serial,
            AssignedUserId = null
        });

        Console.WriteLine("Asset added. Press any key to return to menu.");
        Console.ReadKey();
    }

    static void AssignAsset()
    {
        Console.Clear();
        Console.WriteLine("=== Assign Asset to User ===");

        if (!assets.Any())
        {
            Console.WriteLine("No assets to assign.");
            Console.ReadKey();
            return;
        }

        if (!users.Any())
        {
            Console.WriteLine("No users available. Add a user first.");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Assets:");
        foreach (var asset in assets)
        {
            Console.WriteLine($"{asset.Id}. {asset.Name} ({asset.SerialNumber})");
        }

        Console.Write("Enter Asset ID to assign: ");
        int assetId = int.Parse(Console.ReadLine());

        Console.WriteLine("Users:");
        foreach (var user in users)
        {
            Console.WriteLine($"{user.Id}. {user.Name}");
        }

        Console.Write("Enter User ID to assign asset to: ");
        int userId = int.Parse(Console.ReadLine());

        var assetToAssign = assets.FirstOrDefault(a => a.Id == assetId);
        var userToAssign = users.FirstOrDefault(u => u.Id == userId);

        if (assetToAssign != null && userToAssign != null)
        {
            assetToAssign.AssignedUserId = userId;
            Console.WriteLine("Asset assigned. Press any key to return to menu.");
        }
        else
        {
            Console.WriteLine("Invalid IDs entered.");
        }

        Console.ReadKey();
    }

    static void UpdateAsset()
    {
        Console.Clear();
        Console.WriteLine("=== Update Asset ===");

        Console.Write("Enter Asset ID to update: ");
        int id = int.Parse(Console.ReadLine());

        var asset = assets.FirstOrDefault(a => a.Id == id);
        if (asset == null)
        {
            Console.WriteLine("Asset not found.");
            Console.ReadKey();
            return;
        }

        Console.Write($"New Name (Leave empty to keep '{asset.Name}'): ");
        string newName = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newName))
            asset.Name = newName;

        Console.Write($"New Type (Leave empty to keep '{asset.Type}'): ");
        string newType = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newType))
            asset.Type = newType;

        Console.Write($"New Serial Number (Leave empty to keep '{asset.SerialNumber}'): ");
        string newSerial = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newSerial))
            asset.SerialNumber = newSerial;

        Console.WriteLine("Asset updated. Press any key to return to menu.");
        Console.ReadKey();
    }

    static void DeleteAsset()
    {
        Console.Clear();
        Console.WriteLine("=== Delete Asset ===");

        Console.Write("Enter Asset ID to delete: ");
        int id = int.Parse(Console.ReadLine());

        var asset = assets.FirstOrDefault(a => a.Id == id);
        if (asset != null)
        {
            assets.Remove(asset);
            Console.WriteLine("Asset deleted.");
        }
        else
        {
            Console.WriteLine("Asset not found.");
        }

        Console.WriteLine("Press any key to return to menu.");
        Console.ReadKey();
    }

    static void ViewAssets()
    {
        Console.Clear();
        Console.WriteLine("=== All Assets ===");

        if (!assets.Any())
        {
            Console.WriteLine("No assets found.");
        }
        else
        {
            foreach (var asset in assets)
            {
                var assignedUser = asset.AssignedUserId.HasValue
                    ? users.FirstOrDefault(u => u.Id == asset.AssignedUserId)?.Name
                    : "Unassigned";

                Console.WriteLine($"ID: {asset.Id}");
                Console.WriteLine($"Name: {asset.Name}");
                Console.WriteLine($"Type: {asset.Type}");
                Console.WriteLine($"Serial #: {asset.SerialNumber}");
                Console.WriteLine($"Assigned To: {assignedUser}");
                Console.WriteLine("-------------------------------------");
            }
        }

        Console.WriteLine("Press any key to return to menu.");
        Console.ReadKey();
    }
}