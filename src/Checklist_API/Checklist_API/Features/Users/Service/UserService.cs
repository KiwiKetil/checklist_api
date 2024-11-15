using Checklist_API.Features.Common.Interfaces;
using Checklist_API.Features.Users.DTOs;
using Checklist_API.Features.Users.Entity;
using Checklist_API.Features.Users.Mappers;
using Checklist_API.Features.Users.Repository;
using Checklist_API.Features.Users.Repository.Interfaces;
using Checklist_API.Features.Users.Service.Interfaces;
using static Checklist_API.Features.ExceptionHandling.CustomExceptions;

namespace Checklist_API.Features.Users.Service;

public class UserService(IUserRepository userRepository, ILogger<UserService> logger, IMapper<User, UserDTO> userMapper,
                    IMapper<User, UserRegistrationDTO> userRegistrationMapper) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ILogger<UserService> _logger = logger;
    private readonly IMapper<User, UserDTO> _userMapper = userMapper;
    private readonly IMapper<User, UserRegistrationDTO> _userRegistrationMapper = userRegistrationMapper;

    public async Task<IEnumerable<UserDTO>> GetAllAsync(int page, int pageSize)
    {
        _logger.LogInformation("Retrieving all users");

        var res = await _userRepository.GetAllAsync(page, pageSize);

        var dtos = res.Select(user => _userMapper.MapToDTO(user)).ToList();
        return dtos;
    }

    public async Task<UserDTO?> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Retrieving user with ID: {id}", id);

        var res = await _userRepository.GetByIdAsync(new UserId(id));
        if (res == null) 
        {
            _logger.LogInformation("User not found");
            return null;
        }
        return _userMapper.MapToDTO(res);
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
            _logger.LogInformation("User already exist: {Email}", dto.Email);

            throw new UserAlreadyExistsException();
        }       

        var user = _userRegistrationMapper.MapToEntity(dto);

        user.Id = UserId.NewId;
        user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        user.Salt = BCrypt.Net.BCrypt.GenerateSalt();

        var res = await _userRepository.RegisterAsync(user);

        return res != null ? _userMapper.MapToDTO(res) : null;
    }
}
