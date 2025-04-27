using AssetTracker.Helpers;
using AssetTracker.Views.Menu;

namespace AssetTracker.Controllers;
public class MenuController(UserController userController,
                            AssetsController assetsController)
{
    private readonly UserController _userController = userController;
    private readonly AssetsController _assetsController = assetsController;

    public async Task Run()
    {
        while (true)
        {
            var choice = MenuView.PrintMenuAndGetChoice();

            switch (choice)
            {
                case 1: await _userController.AddNewUser(); break;
                case 2: await _assetsController.AddNewAssetAsync(); break;
                case 3: await _assetsController.AssignAssetAsync(); break;
                case 4: await _assetsController.UpdateAssetAsync(); break;
                case 5: await _assetsController.DeleteAssetAsync(); break;
                case 6: await _assetsController.ViewAllAssetsAsync(); break;
                case 7: await _userController.ViewAllUsersAsync(); break;
                case 8: return;
                default:
                    UiHelpers.PrintErrorMessage("Invalid option.");
                    UiHelpers.PauseDisplay();
                    break;
            }
        }
    }
}