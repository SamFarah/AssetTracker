using AssetTracker.Data.Entities;
using AssetTracker.Data.Repositories;

namespace AssetTracker.Services;
public class UserService(UserRepo users)
{
    private readonly UserRepo _users = users;

    public void AddUser()
    {
        Console.Clear();
        Console.WriteLine("=== Add New User ===");
        Console.Write("User Name: ");
        string name = Console.ReadLine();

        var user = new User
        {
            Name = name
        };

        _users.AddUserToDB(user);

        Console.WriteLine($"User added (Id: {user.Id}). Press any key to return to menu.");
        Console.ReadKey();
    }
}