


using Checklist_API.Features.Users.Entity;

namespace Checklist_API.Features.Users.Repository.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> GetByIdAsync(UserId id);
    Task<User> UpdateAsync(UserId id, User user);
    Task<User> DeleteAsync(UserId id);
    Task<User> RegisterAsync(User user);
}
