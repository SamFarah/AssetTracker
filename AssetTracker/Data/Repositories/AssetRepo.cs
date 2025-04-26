using AssetTracker.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AssetTracker.Data.Repositories;
public class AssetRepo(AssetTrackerDbContext db)
{
    private readonly AssetTrackerDbContext _db = db;

    public async Task AddAssetToDBAsync(Asset asset)
    {
        try
        {
            await _db.Assets.AddAsync(asset);
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {

            throw;
        }

    }

    public async Task<List<Asset>> GetAllAssetsAsync() => await _db.Assets.ToListAsync();

    public async Task<int> AssignAssetToUserAsync(int assetId, int userId)
    {

        var asset = _db.Assets.Find(assetId);
        var user = _db.Users.Find(userId);
        if (asset == null) throw new Exception($"Asset with id {assetId} not found");
        if (user == null) throw new Exception($"User with id {userId} not found");
        asset.AssignedUser = user;
        return await _db.SaveChangesAsync();
    }

    public async Task<int> DeleteAssetAsync(int assetId)
    {
        var asset = _db.Assets.Find(assetId);
        if (asset == null) throw new Exception($"Asset with id {assetId} not found");
        _db.Assets.Remove(asset);
        return await _db.SaveChangesAsync();
    }

    public async Task<Asset> GetAssetByIdAsync(int assetId) => await _db.Assets.FirstOrDefaultAsync(x => x.Id == assetId);

    public async Task UpdateAssetAsync(Asset asset)
    {
        _db.Attach(asset);
        _db.Entry(asset).State = EntityState.Modified;
        await _db.SaveChangesAsync();
    }

}