using AssetTracker.Data.Entities;
using System.Data.SQLite;

namespace AssetTracker.Data.Repositories;
public class UserRepo(AssetTrackerDbContext db)
{
    private readonly AssetTrackerDbContext _db = db;

    public List<User> GetAllAUsers()
    {
        using var conn = _db.GetConnection();
        conn.Open();
        using var cmd = new SQLiteCommand("SELECT Id, Name FROM Users", conn);
        using var userReader = cmd.ExecuteReader();
        List<User> users = [];
        while (userReader.Read())
        {
            users.Add(new()
            {
                Id = userReader.GetInt32(userReader.GetOrdinal("Id")),
                Name = userReader.GetString(userReader.GetOrdinal("Name")),
            });
        }
        userReader.Close();
        return users;
    }

    public void AddUserToDB(User user)
    {
        using var conn = _db.GetConnection();
        conn.Open();

        string sql = "INSERT INTO Users (Name) VALUES (@name)";
        using var cmd = new SQLiteCommand(sql, conn);
        cmd.Parameters.AddWithValue("@name", user.Name);

        cmd.ExecuteNonQuery();
        cmd.CommandText = "select last_insert_rowid()";
        user.Id = (long)cmd.ExecuteScalar();
    }

    public User GetUserById(int userId)
    {
        using var conn = _db.GetConnection();
        conn.Open();

        using var cmd = new SQLiteCommand("SELECT Id, Name FROM Users where Id = @id", conn);
        cmd.Parameters.AddWithValue("@id", userId);
        using var userReader = cmd.ExecuteReader();

        if (userReader.Read())
        {
            return new()
            {
                Id = userReader.GetInt32(userReader.GetOrdinal("Id")),
                Name = userReader.GetString(userReader.GetOrdinal("Name")),
            };
        }
        userReader.Close();
        return null;

    }
}