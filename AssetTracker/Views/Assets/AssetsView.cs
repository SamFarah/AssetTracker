using AssetTracker.Backend.Data.Entities;
using AssetTracker.Helpers;

namespace AssetTracker.Views.Assets;
public class AssetsView
{
    public static void DisplayAsset(Asset asset, bool showAssignedTo = false)
    {
        Console.WriteLine($"\tID: {asset.Id}");
        Console.WriteLine($"\tName: {asset.Name}");
        Console.WriteLine($"\tType: {asset.Type}");
        Console.WriteLine($"\tSerial #: {asset.SerialNumber}");
        if (showAssignedTo) Console.WriteLine($"\tAssigned To: {asset.AssignedUser?.Name ?? "<Unassigned>"}");
        Console.WriteLine("\t-------------------------------------");
    }

    public static void ViewAllAssets(List<Asset> assets)
    {
        Console.Clear();
        Console.WriteLine("=== All Assets ===");
        if (assets.Count == 0) Console.WriteLine("No assets found.");
        else assets.ForEach(asset => DisplayAsset(asset, showAssignedTo: true));
        UiHelpers.PauseDisplay();
    }

    public static Asset GetNewAsset()
    {
        Console.Clear();
        Console.WriteLine("=== Add New Asset ===");
        Console.Write("Asset Name: ");
        string name = Console.ReadLine();

        Console.Write("Asset Type: ");
        string type = Console.ReadLine();

        Console.Write("Serial Number: ");
        string serial = Console.ReadLine();

        return new Asset
        {
            Name = name,
            Type = type,
            SerialNumber = serial,
            AssignedUserId = null
        };
    }

    public static void ConfirmAssetAdded(int assetId)
    {
        UiHelpers.PrintSuccessMessage($"Asset added (Id: {assetId}). Press any key to return to menu.");
        UiHelpers.PauseDisplay();
    }

    public static (bool cancelled, int assetId, int userId) AssignAsseet(List<Asset> assets, List<User> users)
    {
        Console.Clear();
        Console.WriteLine("=== Assign Asset to User ===");

        var assetId = SelectAsset("Select asset to assign: ", assets);
        if (assetId == 0) return (true, 0, 0);

        var userId = SelectUser("Select user to assign: ", users);
        if (userId == 0) return (true, 0, 0);

        return (false, assetId, userId);
    }

    public static int DeleteAsset(List<Asset> assets)
    {
        Console.Clear();
        Console.WriteLine("=== Delete Asset ===");
        return SelectAsset("Select asset to delete: ", assets);
    }

    public static Asset UpdateAsset(List<Asset> assets)
    {
        Console.Clear();
        Console.WriteLine("=== Update Asset ===");
        var assetId = SelectAsset("Select asset to update: ", assets);
        var asset = assets.FirstOrDefault(x => x.Id == assetId);
        if (asset == null) return null;
        Console.Write($"New Name (Leave empty to keep '{asset.Name}'): ");
        string newName = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newName)) asset.Name = newName;

        Console.Write($"New Type (Leave empty to keep '{asset.Type}'): ");
        string newType = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newType)) asset.Type = newType;

        Console.Write($"New Serial Number (Leave empty to keep '{asset.SerialNumber}'): ");
        string newSerial = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(newSerial)) asset.SerialNumber = newSerial;
        return asset;
    }

    private static int SelectAsset(string prompt, List<Asset> assets)
    {
        // Show assets
        Console.WriteLine(prompt);
        assets.ForEach(asset => Console.WriteLine($"{asset.Id}. {asset.Name}"));
        Console.WriteLine("0. Cancel");
        return UiHelpers.GetValidInt("Enter Asset ID to assign: ", 0, assets.Max(x => x.Id));
    }

    private static int SelectUser(string prompt, List<User> users)
    {
        // Show users
        Console.WriteLine(prompt);
        users.ForEach(user => Console.WriteLine($"{user.Id}. {user.Name}"));
        Console.WriteLine("0. Cancel");
        return UiHelpers.GetValidInt("Enter User ID to assign asset to: ", 0, users.Max(x => x.Id));
    }

}
