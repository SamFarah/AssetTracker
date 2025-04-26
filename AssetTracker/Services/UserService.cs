using AssetTracker.Data.Entities;
using AssetTracker.Data.Repositories;

namespace AssetTracker.Services;
public class UserService(UserRepo users)
{
    private readonly UserRepo _users = users;

    public async Task AddUserAsync()
    {
        Console.Clear();
        Console.WriteLine("=== Add New User ===");
        Console.Write("User Name: ");
        string name = Console.ReadLine();

        var user = new User
        {
            Name = name
        };

        await _users.AddUserToDBAsync(user);

        Console.WriteLine($"User added (Id: {user.Id}). Press any key to return to menu.");
        Console.ReadKey();
    }

    public async Task ViewUsersAsync()
    {
        Console.Clear();
        Console.WriteLine("=== All Users ===");

        var users = await _users.GetAllAUsersAsync();

        if (users.Count == 0) Console.WriteLine("No users found.");
        else
        {
            foreach (var user in users)
            {
                Console.WriteLine($"ID: {user.Id}");
                Console.WriteLine($"Name: {user.Name}");
                Console.WriteLine($"Assigned Assets:");
                foreach (var asset in user.AssignedAssets)
                {
                    Console.WriteLine($"\tID: {asset.Id}");
                    Console.WriteLine($"\tName: {asset.Name}");
                    Console.WriteLine($"\tType: {asset.Type}");
                    Console.WriteLine($"\tSerial #: {asset.SerialNumber}");                    
                    Console.WriteLine("\t-------------------------------------");
                }
                Console.WriteLine("-------------------------------------");
            }
        }

        Console.WriteLine("Press any key to return to menu.");
        Console.ReadKey();
    }
}