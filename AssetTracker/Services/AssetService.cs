﻿using AssetTracker.Data.Entities;
using AssetTracker.Data.Repositories;

namespace AssetTracker.Services;
public class AssetService(AssetRepo assets, UserRepo users)
{
    private readonly AssetRepo _assets = assets;
    private readonly UserRepo _users = users;

    public void ViewAssets()
    {
        Console.Clear();
        Console.WriteLine("=== All Assets ===");

        var assets = _assets.GetAllAssets();

        if (assets.Count == 0) Console.WriteLine("No assets found.");
        else
        {
            foreach (var asset in assets)
            {
                Console.WriteLine($"ID: {asset.Id}");
                Console.WriteLine($"Name: {asset.Name}");
                Console.WriteLine($"Type: {asset.Type}");
                Console.WriteLine($"Serial #: {asset.SerialNumber}");
                Console.WriteLine($"Assigned To: {asset.AssignedUser?.Name ?? "Unassigned"}");
                Console.WriteLine("-------------------------------------");
            }
        }

        Console.WriteLine("Press any key to return to menu.");
        Console.ReadKey();
    }

    public void AddAsset()
    {
        Console.Clear();
        Console.WriteLine("=== Add New Asset ===");
        Console.Write("Asset Name: ");
        string name = Console.ReadLine();

        Console.Write("Asset Type: ");
        string type = Console.ReadLine();

        Console.Write("Serial Number: ");
        string serial = Console.ReadLine();

        var asset = new Asset
        {
            Name = name,
            Type = type,
            SerialNumber = serial,
            AssignedUserId = null
        };

        _assets.AddAssetToDB(asset);

        Console.WriteLine($"Asset added (Id: {asset.Id}). Press any key to return to menu.");
        Console.ReadKey();
    }

    public void AssignAsset()
    {
        Console.Clear();
        Console.WriteLine("=== Assign Asset to User ===");


        // Show assets
        Console.WriteLine("Assets:");
        var assets = _assets.GetAllAssets();
        assets.ForEach(asset => Console.WriteLine($"{asset.Id}. {asset.Name}"));


        Console.Write("Enter Asset ID to assign: ");
        int assetId = int.Parse(Console.ReadLine());

        // Show users
        Console.WriteLine("Users:");
        var users = _users.GetAllAUsers();
        users.ForEach(user => Console.WriteLine($"{user.Id}. {user.Name}"));


        Console.Write("Enter User ID to assign asset to: ");
        int userId = int.Parse(Console.ReadLine());

        // Update

        var affected = _assets.AssignAssetToUser(assetId, userId);
        Console.WriteLine(affected > 0 ? "Asset assigned." : "Assignment failed.");
        Console.WriteLine("Press any key to return to menu.");
        Console.ReadKey();
    }


    public void DeleteAsset()
    {
        Console.Clear();
        Console.WriteLine("=== Delete Asset ===");

        Console.Write("Enter Asset ID to delete: ");
        int id = int.Parse(Console.ReadLine());

        var affected = _assets.DeleteAsset(id);

        Console.WriteLine(affected > 0 ? "Asset deleted." : "Asset not found.");
        Console.WriteLine("Press any key to return to menu.");
        Console.ReadKey();
    }

    public void UpdateAsset()
    {
        Console.Clear();
        Console.WriteLine("=== Update Asset ===");

        Console.Write("Enter Asset ID to update: ");
        int id = int.Parse(Console.ReadLine());


        var asset = _assets.GetAssetById(id);
        if (asset == null)
        {
            Console.WriteLine("Asset not found.");
            Console.ReadKey();
            return;
        }

        Console.Write($"New Name (Leave empty to keep '{asset.Name}'): ");
        string newName = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newName)) asset.Name = newName;

        Console.Write($"New Type (Leave empty to keep '{asset.Type}'): ");
        string newType = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newType)) asset.Type = newType;

        Console.Write($"New Serial Number (Leave empty to keep '{asset.SerialNumber}'): ");
        string newSerial = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newSerial)) asset.SerialNumber = newSerial;

        _assets.UpdateAsset(asset);

        Console.WriteLine("Asset updated. Press any key to return to menu.");
        Console.ReadKey();
    }
}