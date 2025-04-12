using AssetTracker.Data.Entities;
using System.Data.SQLite;

namespace AssetTracker.Data.Repositories;
public class AssetRepo(AssetTrackerDbContext db)
{
    private readonly AssetTrackerDbContext _db = db;

    public void AddAssetToDB(Asset asset)
    {
        using var conn = _db.GetConnection();
        conn.Open();

        string sql = "INSERT INTO Assets (Name, Type, SerialNumber) VALUES (@name, @type, @serial)";
        using var cmd = new SQLiteCommand(sql, conn);
        cmd.Parameters.AddWithValue("@name", asset.Name);
        cmd.Parameters.AddWithValue("@type", asset.Type);
        cmd.Parameters.AddWithValue("@serial", asset.SerialNumber);
        cmd.ExecuteNonQuery();
        cmd.CommandText = "select last_insert_rowid()";
        asset.Id = (long)cmd.ExecuteScalar();
    }

    public List<Asset> GetAllAssets()
    {
        using var conn = _db.GetConnection();
        conn.Open();
        using var assetCmd = new SQLiteCommand(@"SELECT A.Id, A.Name, A.Type, A.SerialNumber, A.AssignedUserId,
                                                      U.Name AS AssignedTo
                                               FROM Assets A
                                               LEFT JOIN Users U ON A.AssignedUserId = U.Id", conn);
        using var assetReader = assetCmd.ExecuteReader();
        List<Asset> assets = [];
        while (assetReader.Read()) assets.Add(GetAssetFromReader(assetReader));

        assetReader.Close();
        return assets;
    }

    private Asset GetAssetFromReader(SQLiteDataReader assetReader)
    {
        var asset = new Asset()
        {
            Id = assetReader.GetInt32(assetReader.GetOrdinal("Id")),
            Name = assetReader.GetString(assetReader.GetOrdinal("Name")),
            SerialNumber = assetReader.GetString(assetReader.GetOrdinal("SerialNumber")),
            Type = assetReader.GetString(assetReader.GetOrdinal("Type"))
        };
        int colIndex = assetReader.GetOrdinal("AssignedUserId");
        if (!assetReader.IsDBNull(colIndex))
        {
            asset.AssignedUserId = assetReader.GetInt32(assetReader.GetOrdinal("AssignedUserId"));
            asset.AssignedUser = new()
            {
                Id = assetReader.GetInt32(assetReader.GetOrdinal("Id")),
                Name = assetReader.GetString(assetReader.GetOrdinal("Name")),
            };
        }
        return asset;
    }

    public int AssignAssetToUser(int assetId, int userId)
    {
        using var conn = _db.GetConnection();
        conn.Open();
        string sql = "UPDATE Assets SET AssignedUserId = @userId WHERE Id = @assetId";
        using var updateCmd = new SQLiteCommand(sql, conn);
        updateCmd.Parameters.AddWithValue("@userId", userId);
        updateCmd.Parameters.AddWithValue("@assetId", assetId);
        return updateCmd.ExecuteNonQuery();
    }

    public int DeleteAsset(int assetId)
    {
        using var conn = _db.GetConnection();
        conn.Open();
        string sql = "DELETE FROM Assets WHERE Id = @id";
        using var cmd = new SQLiteCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", assetId);
        return cmd.ExecuteNonQuery();
    }

    public Asset GetAssetById(int assetId)
    {
        using var conn = _db.GetConnection();
        conn.Open();
        using var assetCmd = new SQLiteCommand(@"SELECT A.Id, A.Name, A.Type, A.SerialNumber, A.AssignedUserId,
                                                      U.Name AS AssignedTo
                                               FROM Assets A
                                               LEFT JOIN Users U ON A.AssignedUserId = U.Id
                                               WHERE A.Id = @id", conn);
        assetCmd.Parameters.AddWithValue("@id", assetId);
        using var assetReader = assetCmd.ExecuteReader();
        while (assetReader.Read()) return GetAssetFromReader(assetReader);

        assetReader.Close();
        return null;
    }

    public void UpdateAsset(Asset asset)
    {
        using var conn = _db.GetConnection();
        conn.Open();
        string updateSql = @"UPDATE Assets 
                         SET Name = @name, Type = @type, SerialNumber = @serial 
                         WHERE Id = @id";

        using var updateCmd = new SQLiteCommand(updateSql, conn);
        updateCmd.Parameters.AddWithValue("@name", asset.Name);
        updateCmd.Parameters.AddWithValue("@type", asset.Type);
        updateCmd.Parameters.AddWithValue("@serial", asset.SerialNumber);
        updateCmd.Parameters.AddWithValue("@id", asset.Id);

        updateCmd.ExecuteNonQuery();
    }

}