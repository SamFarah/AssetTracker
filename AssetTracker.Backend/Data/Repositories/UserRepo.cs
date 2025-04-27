using AssetTracker.Backend.Data.Entities;
using Microsoft.EntityFrameworkCore;


namespace AssetTracker.Backend.Data.Repositories;
public class UserRepo(AssetTrackerDbContext db)
{
    private readonly AssetTrackerDbContext _db = db;

    public async Task<List<User>> GetAllAUsersAsync() => await _db.Users.ToListAsync();

    public async Task AddUserToDBAsync(User user)
    {
        try
        {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
        }
        catch (Exception )// place holder for now
        {
            throw;
        }
    }

    public async Task<User> GetUserByIdAsync(int userId) =>
        await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);

}