using AssetTracker.Backend.Services;
using AssetTracker.Views.Users;

namespace AssetTracker.Controllers;
public class UserController(UserService userService)
{
    private readonly UserService _userService = userService;

    public async Task AddNewUser()
    {
        var newUser = UsersView.GetNewUser();
        await _userService.AddUserAsync(newUser);
        UsersView.ConfirmUserAdded(newUser.Id);
    }

    public async Task ViewAllUsersAsync()
    {
        var users = await _userService.GetAllUsersAsync();
        UsersView.ViewAllUsers(users);
    }
}
