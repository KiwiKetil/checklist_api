using Checklist_API.Features.Users.Entity;

namespace Checklist_API.Features.Users.Repository.Interfaces;

public class UserRepository : IUserRepository
{
    public Task<User> DeleteAsync(UserId id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<User>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<User> GetByIdAsync(UserId id)
    {
        throw new NotImplementedException();
    }

    public Task<User> RegisterAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task<User> UpdateAsync(UserId id, User user)
    {
        throw new NotImplementedException();
    }
}
