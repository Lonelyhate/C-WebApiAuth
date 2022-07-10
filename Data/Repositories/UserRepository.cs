using apiLeran.Interfaces;
using apiLeran.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace apiLeran.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task AddRole(IdentityUserRole<string> identityRole)
    {
        await _dbContext.UserRoles.AddAsync(identityRole);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IdentityRole> GetRole(string role)
    {
        return await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == role);
    }

    public async Task<IEnumerable<User>> GetUsers()
    {
        var users = await _dbContext.Users.ToListAsync();

        if (users is not null)
        {
            return users;
        }

        return null;
    }

    public async Task<User> GetByEmail(string email)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user is not null) return user;

        return null;
    }

    public async Task<User> GetByName(string userName)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Name == userName);
        
        if (user is not null) return user;
        
        return null;
    }

    public async Task<User> GetById(string id)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (user is not null) return user;

        return null;
    }

    public async Task Add(User user)
    {
        await _dbContext.Users.AddAsync(user);
    }

    public async Task<User> Edit(User user, string name)
    {
        var _user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

        if (user is not null)
        {
            _user.Name = name;
            _dbContext.Update(user);

            return _user;
        }

        return null;
    }

    public async Task Delete(string id)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (user is not null) _dbContext.Users.Remove(user);

        await _dbContext.SaveChangesAsync();
    }

    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }
}