using Checklist_API.Data;
using Checklist_API.Features.JWT.Entity;
using Checklist_API.Features.Login.DTOs;
using Checklist_API.Features.Users.Entity;
using Checklist_API.Features.Users.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Checklist_API.Features.Users.Repository;

public class UserRepository(CheckListDbContext dbContext, ILogger<UserRepository> logger) : IUserRepository
{
    private readonly CheckListDbContext _dbContext = dbContext;
    private readonly ILogger<UserRepository> _logger = logger;

    public async Task<IEnumerable<User>> GetAllUsersAsync(int page, int pageSize)
    {
        _logger.LogInformation("Retrieving users from db");

        int itemToSkip = (page - 1) * pageSize;

        return await _dbContext.User
            .OrderBy(x => x.LastName)
            .Skip(itemToSkip)
            .Take(pageSize)
            .Distinct()
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(UserId id)
    {
        _logger.LogInformation("Retrieving user with ID: {id}", id);

        return await _dbContext.User.FirstOrDefaultAsync(c => c.Id == id);
    }

    public Task<User?> UpdateUserAsync(UserId id, User user)
    {
        throw new NotImplementedException();
    }

    public Task<User?> DeleteUserAsync(UserId id)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> RegisterUserAsync(User user)
    {
        _logger.LogInformation("Adding user: {user} to db", user.Email);

        var res = await _dbContext.User.AddAsync(user);

        UserRole roleAssignment = new()
        {
            RoleName = "User",
            UserId = user.Id,            
            DateCreated = DateTime.Now,
            DateUpdated = DateTime.Now
        }; 

        await _dbContext.UserRole.AddAsync(roleAssignment);
        await _dbContext.SaveChangesAsync();

        return res.Entity;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        _logger.LogInformation("Retrieving user by email: {email} from db", email);

        var res = await _dbContext.User.FirstOrDefaultAsync(x => x.Email.Equals(email));
        return res;
    }
}
