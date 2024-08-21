using Checklist_API.Features.Common.Interfaces;
using Checklist_API.Features.Users.DTOs;
using Checklist_API.Features.Users.Entity;
using Checklist_API.Features.Users.Mappers;
using Checklist_API.Features.Users.Repository.Interfaces;
using Checklist_API.Features.Users.Service.Interfaces;

namespace Checklist_API.Features.Users.Service;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;
    private readonly IMapper<User, UserDTO> _userMapper;

    public UserService(IUserRepository userRepository, ILogger<UserService> logger, IMapper<User, UserDTO> userMapper)
    {
        _userRepository = userRepository;
        _logger = logger;
        _userMapper = userMapper;
    }

    public async Task<IEnumerable<UserDTO>> GetAllAsync(int page, int pageSize)
    {
        _logger.LogInformation("getting all users");

        var res = await _userRepository.GetAllAsync(page, pageSize);

        var dtos = res.Select(user => _userMapper.MapToDTO(user)).ToList();
        return dtos;
    }

    public Task<UserDTO?> GetByIdAsync(Guid UserId)
    {
        throw new NotImplementedException();
    }

    public Task<UserDTO?> UpdateAsync(Guid id, UserDTO dto)
    {
        throw new NotImplementedException();
    }

    public Task<UserDTO?> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<UserDTO?> RegisterUserAsync(UserRegistrationDTO dto)
    {
        _logger.LogDebug("Registering new user: {email}", dto.Email);

        var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
        if (existingUser != null)
        {
            _logger.LogDebug("User already exist: {Email}", dto.Email);
            return null;
        }
        
        var user = _
    }
}
