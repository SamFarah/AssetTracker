using AssetTracker.Backend.Data.Entities;
using AssetTracker.Backend.Data.Repositories;

namespace AssetTracker.Backend.Services;
public class UserService(UserRepo users)
{
    private readonly UserRepo _users = users;

    public async Task AddUserAsync(User user) => await _users.AddUserToDBAsync(user);
    public async Task<List<User>> GetAllUsersAsync() => await _users.GetAllAUsersAsync();
}