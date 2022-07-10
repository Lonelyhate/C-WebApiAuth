using apiLeran.Models;
using Microsoft.AspNetCore.Identity;

namespace apiLeran.Interfaces;

public interface IUserRepository
{
    Task AddRole(IdentityUserRole<string> identityUserRole);

    Task<IdentityRole> GetRole(string role);

    Task<IEnumerable<User>> GetUsers();

    Task<User> GetByEmail(string email);

    Task<User> GetById(string id);

    Task<User> GetByName(string userName);

    Task Add(User user);

    Task<User> Edit(User user, string name);

    Task Delete(string id);

    Task Save();
}