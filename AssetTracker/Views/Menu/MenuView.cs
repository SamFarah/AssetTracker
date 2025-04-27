using AssetTracker.Helpers;

namespace AssetTracker.Views.Menu;
public class MenuView
{
    private static readonly List<string> _menuItems = [
        "1. Add User",
        "2. Add Asset",
        "3. Assign Asset to User",
        "4. Update Asset",
        "5. Delete Asset",
        "6. View All Assets",
        "7. View All Users",
        "8. Exit"
    ];

    public static int PrintMenuAndGetChoice()
    {
        Console.Clear();
        Console.WriteLine("=== Assetrix - Asset Management ===");
        _menuItems.ForEach(Console.WriteLine);
        return UiHelpers.GetValidInt("Choose an option: ", 1, _menuItems.Count);
    }

}
