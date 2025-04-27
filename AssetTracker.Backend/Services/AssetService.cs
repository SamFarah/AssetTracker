using AssetTracker.Backend.Data.Entities;
using AssetTracker.Backend.Data.Repositories;

namespace AssetTracker.Backend.Services;
public class AssetService(AssetRepo assets, UserRepo users)
{
    private readonly AssetRepo _assets = assets;
    private readonly UserRepo _users = users;

    public async Task<List<Asset>> GetAllAssetsAsync() => await _assets.GetAllAssetsAsync();

    public async Task AddAssetAsync(Asset asset) => await _assets.AddAssetToDBAsync(asset);

    public async Task<int> AssignAssetAsync(int assetId, int userId) =>
        await _assets.AssignAssetToUserAsync(assetId, userId);
    public async Task UpdateAssetAsync(Asset asset) => await _assets.UpdateAssetAsync(asset);

    public async Task<int> DeleteAssetAsync(int assetId) => await _assets.DeleteAssetAsync(assetId);

}