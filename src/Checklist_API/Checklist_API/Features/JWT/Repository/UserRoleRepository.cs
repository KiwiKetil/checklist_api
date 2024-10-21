using Check_List_API.Data;
using Checklist_API.Features.JWT.Entity;
using Checklist_API.Features.JWT.Repository.Interfaces;
using Checklist_API.Features.Users.Entity;
using Microsoft.EntityFrameworkCore;

namespace Checklist_API.Features.JWT.Repository;

public class UserRoleRepository(CheckListDbContext dbContext, ILogger<UserRoleRepository> logger) : IUserRoleRepository
{
    private readonly CheckListDbContext _dbContext = dbContext;
    private readonly ILogger<UserRoleRepository> _logger = logger;   

    public async Task<IEnumerable<UserRole>> GetUserRolesAsync(UserId id)
    {
        _logger.LogInformation("Retrieving roles for user with ID: {UserId}", id);

        var userRoles = await _dbContext.UserRole.Where(ur => ur.UserId == id).AsNoTracking().ToListAsync();
        return userRoles;
    }
}
