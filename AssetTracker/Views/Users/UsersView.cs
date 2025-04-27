using AssetTracker.Backend.Data.Entities;
using AssetTracker.Helpers;
using AssetTracker.Views.Assets;

namespace AssetTracker.Views.Users;
public class UsersView
{
    public static User GetNewUser()
    {
        Console.Clear();
        Console.WriteLine("=== Add New User ===");
        Console.Write("User Name: ");

        string name = Console.ReadLine();

        return new User
        {
            Name = name
        };
    }

    public static void ConfirmUserAdded(int userId)
    {
        UiHelpers.PrintSuccessMessage($"User added (Id: {userId}). Press any key to return to menu.");
        UiHelpers.PauseDisplay();
    }

    public static void DisplayUser(User user)
    {
        Console.WriteLine($"ID: {user.Id}");
        Console.WriteLine($"Name: {user.Name}");
        Console.WriteLine($"Assigned Assets:");
        user.AssignedAssets?.ForEach(asset => AssetsView.DisplayAsset(asset));
        Console.WriteLine("-------------------------------------");
    }

    public static void ViewAllUsers(List<User> users)
    {
        Console.Clear();
        Console.WriteLine("=== All Users ===");
        if (users.Count == 0) Console.WriteLine("No users found.");
        else users.ForEach(DisplayUser);
        UiHelpers.PauseDisplay();
    }
}
