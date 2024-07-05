


using Checklist_API.Features.Users.Entity;

namespace Checklist_API.Features.Users.Repository.Interfaces;

public interface IUserRepository
{
    Task<ICollection<User>> GetAllAsync();
    Task<ICollection<User>> GetByIdAsync(UserId id);
    Task<ICollection<User>> UpdateAsync(UserId id, User user);
    Task<ICollection<User>> DeleteAsync(UserId id);
    Task<ICollection<User>> RegisterAsync(User user);
}
