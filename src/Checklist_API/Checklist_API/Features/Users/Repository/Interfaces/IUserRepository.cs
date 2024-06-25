


using Checklist_API.Features.Users.Entity;

namespace Checklist_API.Features.Users.Repository.Interfaces;

public interface IUserRepository
{
    Task<ICollection<User>> GetAllAsync();
  //  Task<ICollection<User> GetAllAsync();
}
