using System.Data.SQLite;

namespace AssetTracker;

class Program
{

    static string connectionString = "Data Source=AssetTracker.db;Version=3;";


    class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    class Asset
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string SerialNumber { get; set; }
        public int? AssignedUserId { get; set; }
    }

    //static List<User> users = new List<User>();
    //static List<Asset> assets = new List<Asset>();


    static void Main(string[] args)
    {
        EnsureDatabase();
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Assetrix - Asset Management ===");
            Console.WriteLine("1. Add User");
            Console.WriteLine("2. Add Asset");
            Console.WriteLine("3. Assign Asset to User");
            Console.WriteLine("4. Update Asset");
            Console.WriteLine("5. Delete Asset");
            Console.WriteLine("6. View All Assets");
            Console.WriteLine("7. Exit");
            Console.Write("Choose an option: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1": AddUser(); break;
                case "2": AddAsset(); break;
                case "3": AssignAsset(); break;
                case "4": UpdateAsset(); break;
                case "5": DeleteAsset(); break;
                case "6": ViewAssets(); break;
                case "7": return;
                default:
                    Console.WriteLine("Invalid option. Press any key to try again.");
                    Console.ReadKey();
                    break;
            }
        }
    }

    /// <summary>
    /// Create a database schama if it doenst exist
    /// </summary>
    static void EnsureDatabase()
    {
        using var conn = new SQLiteConnection(connectionString);
        conn.Open();

        string createUsers = @"CREATE TABLE IF NOT EXISTS Users (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL
            );";

        string createAssets = @"CREATE TABLE IF NOT EXISTS Assets (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Type TEXT,
                SerialNumber TEXT,
                AssignedUserId INTEGER,
                FOREIGN KEY (AssignedUserId) REFERENCES Users(Id)
            );";

        using var cmd1 = new SQLiteCommand(createUsers, conn);
        using var cmd2 = new SQLiteCommand(createAssets, conn);
        cmd1.ExecuteNonQuery();
        cmd2.ExecuteNonQuery();
    }

    

    static void AddUser()
    {
        Console.Clear();
        Console.WriteLine("=== Add New User ===");
        Console.Write("User Name: ");
        string name = Console.ReadLine();

        var user = new User
        {
            Name = name
        };

        using var conn = new SQLiteConnection(connectionString);
        conn.Open();

        string sql = "INSERT INTO Users (Name) VALUES (@name)";
        using var cmd = new SQLiteCommand(sql, conn);
        cmd.Parameters.AddWithValue("@name", user.Name);
        cmd.ExecuteNonQuery();
        cmd.CommandText = "select last_insert_rowid()";
        user.Id = (long)cmd.ExecuteScalar();

        Console.WriteLine($"User added (Id: {user.Id}). Press any key to return to menu.");
        Console.ReadKey();
    }


    static void AddAsset()
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

        using var conn = new SQLiteConnection(connectionString);
        conn.Open();

        string sql = "INSERT INTO Assets (Name, Type, SerialNumber) VALUES (@name, @type, @serial)";
        using var cmd = new SQLiteCommand(sql, conn);
        cmd.Parameters.AddWithValue("@name", asset.Name);
        cmd.Parameters.AddWithValue("@type", asset.Type);
        cmd.Parameters.AddWithValue("@serial", asset.SerialNumber);
        cmd.ExecuteNonQuery();
        cmd.CommandText = "select last_insert_rowid()";
        asset.Id = (long)cmd.ExecuteScalar();

        Console.WriteLine($"Asset added (Id: {asset.Id}). Press any key to return to menu.");
        Console.ReadKey();
    }



    static void AssignAsset()
    {
        Console.Clear();
        Console.WriteLine("=== Assign Asset to User ===");

        using var conn = new SQLiteConnection(connectionString);
        conn.Open();

        // Show assets
        Console.WriteLine("Assets:");
        using var assetCmd = new SQLiteCommand("SELECT Id, Name FROM Assets", conn);
        using var assetReader = assetCmd.ExecuteReader();
        while (assetReader.Read())
        {
            Console.WriteLine($"{assetReader["Id"]}. {assetReader["Name"]}");
        }
        assetReader.Close();

        Console.Write("Enter Asset ID to assign: ");
        int assetId = int.Parse(Console.ReadLine());

        // Show users
        Console.WriteLine("Users:");
        using var userCmd = new SQLiteCommand("SELECT Id, Name FROM Users", conn);
        using var userReader = userCmd.ExecuteReader();
        while (userReader.Read())
        {
            Console.WriteLine($"{userReader["Id"]}. {userReader["Name"]}");
        }
        userReader.Close();

        Console.Write("Enter User ID to assign asset to: ");
        int userId = int.Parse(Console.ReadLine());

        // Update
        string sql = "UPDATE Assets SET AssignedUserId = @userId WHERE Id = @assetId";
        using var updateCmd = new SQLiteCommand(sql, conn);
        updateCmd.Parameters.AddWithValue("@userId", userId);
        updateCmd.Parameters.AddWithValue("@assetId", assetId);
        int affected = updateCmd.ExecuteNonQuery();

        Console.WriteLine(affected > 0 ? "Asset assigned." : "Assignment failed.");
        Console.WriteLine("Press any key to return to menu.");
        Console.ReadKey();
    }


    static void UpdateAsset()
    {
        Console.Clear();
        Console.WriteLine("=== Update Asset ===");

        Console.Write("Enter Asset ID to update: ");
        int id = int.Parse(Console.ReadLine());

        using var conn = new SQLiteConnection(connectionString);
        conn.Open();

        string getSql = "SELECT Name, Type, SerialNumber FROM Assets WHERE Id = @id";
        using var getCmd = new SQLiteCommand(getSql, conn);
        getCmd.Parameters.AddWithValue("@id", id);

        using var reader = getCmd.ExecuteReader();
        if (!reader.Read())
        {
            Console.WriteLine("Asset not found.");
            Console.ReadKey();
            return;
        }

        string oldName = reader["Name"].ToString();
        string oldType = reader["Type"].ToString();
        string oldSerial = reader["SerialNumber"].ToString();
        reader.Close();

        Console.Write($"New Name (Leave empty to keep '{oldName}'): ");
        string newName = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(newName)) newName = oldName;

        Console.Write($"New Type (Leave empty to keep '{oldType}'): ");
        string newType = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(newType)) newType = oldType;

        Console.Write($"New Serial Number (Leave empty to keep '{oldSerial}'): ");
        string newSerial = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(newSerial)) newSerial = oldSerial;

        string updateSql = @"UPDATE Assets 
                         SET Name = @name, Type = @type, SerialNumber = @serial 
                         WHERE Id = @id";

        using var updateCmd = new SQLiteCommand(updateSql, conn);
        updateCmd.Parameters.AddWithValue("@name", newName);
        updateCmd.Parameters.AddWithValue("@type", newType);
        updateCmd.Parameters.AddWithValue("@serial", newSerial);
        updateCmd.Parameters.AddWithValue("@id", id);

        updateCmd.ExecuteNonQuery();

        Console.WriteLine("Asset updated. Press any key to return to menu.");
        Console.ReadKey();
    }


    static void DeleteAsset()
    {
        Console.Clear();
        Console.WriteLine("=== Delete Asset ===");

        Console.Write("Enter Asset ID to delete: ");
        int id = int.Parse(Console.ReadLine());

        using var conn = new SQLiteConnection(connectionString);
        conn.Open();

        string sql = "DELETE FROM Assets WHERE Id = @id";
        using var cmd = new SQLiteCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", id);
        int affected = cmd.ExecuteNonQuery();

        Console.WriteLine(affected > 0 ? "Asset deleted." : "Asset not found.");
        Console.WriteLine("Press any key to return to menu.");
        Console.ReadKey();
    }


    static void ViewAssets()
    {
        Console.Clear();
        Console.WriteLine("=== All Assets ===");

        using var conn = new SQLiteConnection(connectionString);
        conn.Open();

        string sql = @"SELECT A.Id, A.Name, A.Type, A.SerialNumber, 
                          U.Name AS AssignedTo
                   FROM Assets A
                   LEFT JOIN Users U ON A.AssignedUserId = U.Id";

        using var cmd = new SQLiteCommand(sql, conn);
        using var reader = cmd.ExecuteReader();

        if (!reader.HasRows)
        {
            Console.WriteLine("No assets found.");
        }
        else
        {
            while (reader.Read())
            {
                Console.WriteLine($"ID: {reader["Id"]}");
                Console.WriteLine($"Name: {reader["Name"]}");
                Console.WriteLine($"Type: {reader["Type"]}");
                Console.WriteLine($"Serial #: {reader["SerialNumber"]}");
                Console.WriteLine($"Assigned To: {reader["AssignedTo"] ?? "Unassigned"}");
                Console.WriteLine("-------------------------------------");
            }
        }

        Console.WriteLine("Press any key to return to menu.");
        Console.ReadKey();
    }

}