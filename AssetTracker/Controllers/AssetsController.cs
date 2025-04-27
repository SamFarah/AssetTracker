using AssetTracker.Backend.Services;
using AssetTracker.Helpers;
using AssetTracker.Views.Assets;

namespace AssetTracker.Controllers;
public class AssetsController(AssetService assetService, UserService userService)
{
    private readonly AssetService _assetService = assetService;
    private readonly UserService _userService = userService;

    public async Task ViewAllAssetsAsync()
    {
        var assets = await _assetService.GetAllAssetsAsync();
        AssetsView.ViewAllAssets(assets);
    }

    public async Task AddNewAssetAsync()
    {
        var newAsset = AssetsView.GetNewAsset();
        await _assetService.AddAssetAsync(newAsset);
        AssetsView.ConfirmAssetAdded(newAsset.Id);
    }

    public async Task AssignAssetAsync()
    {
        var assets = await _assetService.GetAllAssetsAsync();
        var users = await _userService.GetAllUsersAsync();

        var (cancelled, assetId, userId) = AssetsView.AssignAsseet(assets, users);
        if (cancelled) UiHelpers.PrintErrorMessage("Operation canceled");
        else
        {
            try
            {
                var affected = await _assetService.AssignAssetAsync(assetId, userId);
                if (affected > 0) UiHelpers.PrintSuccessMessage("Asset assigned.");
                else UiHelpers.PrintErrorMessage("Assignment failed.");
            }
            catch (Exception ex) { UiHelpers.PrintErrorMessage(ex.Message); }
        }
        UiHelpers.PauseDisplay();
    }

    public async Task UpdateAssetAsync()
    {
        var assets = await _assetService.GetAllAssetsAsync();
        var updatedAsset = AssetsView.UpdateAsset(assets);
        if (updatedAsset == null) UiHelpers.PrintErrorMessage("Asset not found.");
        else
        {
            try
            {
                await _assetService.UpdateAssetAsync(updatedAsset);
                UiHelpers.PrintSuccessMessage("Asset updated.");
            }
            catch (Exception ex) { UiHelpers.PrintErrorMessage(ex.Message); }
        }
        UiHelpers.PauseDisplay();
    }

    public async Task DeleteAssetAsync()
    {
        var assets = await _assetService.GetAllAssetsAsync();
        var assetId = AssetsView.DeleteAsset(assets);
        if (assetId == 0) UiHelpers.PrintErrorMessage("Operation canceled");
        else
        {
            try
            {
                var affected = await _assetService.DeleteAssetAsync(assetId);
                if (affected > 0) UiHelpers.PrintSuccessMessage("Asset deleted.");
                else UiHelpers.PrintErrorMessage("deletion failed.");
            }
            catch (Exception ex) { UiHelpers.PrintErrorMessage(ex.Message); }
        }
        UiHelpers.PauseDisplay();
    }
}
