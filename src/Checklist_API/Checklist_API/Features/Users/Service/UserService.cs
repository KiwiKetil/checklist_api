using Checklist_API.Features.Users.DTOs;
using Checklist_API.Features.Users.Repository.Interfaces;
using Checklist_API.Features.Users.Service.Interfaces;

namespace Checklist_API.Features.Users.Service;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<UserDTO>> GetAllAsync()
    {
        _logger.LogInformation("getting all users");

        await Task.Delay(10); // remove this

        return Enumerable.Empty<UserDTO>(); //husk bytte
    }

    public Task<UserDTO> GetByIdAsync(Guid UserId)
    {
        throw new NotImplementedException();
    }

    public Task<UserDTO> UpdateAsync(Guid id, UserDTO dto)
    {
        throw new NotImplementedException();
    }

    public Task<UserDTO> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<UserDTO> RegisterUserAsync(UserRegistrationDTO dto)
    {
        throw new NotImplementedException();
    }
}
