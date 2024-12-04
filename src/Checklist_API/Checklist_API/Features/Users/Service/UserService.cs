using Checklist_API.Features.Common.Interfaces;
using Checklist_API.Features.Users.DTOs;
using Checklist_API.Features.Users.Entity;
using Checklist_API.Features.Users.Mappers;
using Checklist_API.Features.Users.Repository;
using Checklist_API.Features.Users.Repository.Interfaces;
using Checklist_API.Features.Users.Service.Interfaces;
using static Checklist_API.Features.ExceptionHandling.CustomExceptions;

namespace Checklist_API.Features.Users.Service;

public class UserService(IUserRepository userRepository, ILogger<UserService> logger, 
                    IMapper<User, UserDTO> userMapper,
                    IMapper<User, UserUpdateDTO> userUpdateMapper,
                    IMapper<User, UserRegistrationDTO> userRegistrationMapper) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ILogger<UserService> _logger = logger;
    private readonly IMapper<User, UserDTO> _userMapper = userMapper;
    private readonly IMapper<User, UserUpdateDTO> _userUpdateMapper = userUpdateMapper;
    private readonly IMapper<User, UserRegistrationDTO> _userRegistrationMapper = userRegistrationMapper;

    public async Task<IEnumerable<UserDTO>> GetAllUsersAsync(int page, int pageSize)
    {
        var res = await _userRepository.GetAllUsersAsync(page, pageSize);

        var dtos = res.Select(user => _userMapper.MapToDTO(user)).ToList();
        return dtos;
    }

    public async Task<UserDTO?> GetUserByIdAsync(Guid id)
    {
        var res = await _userRepository.GetUserByIdAsync(new UserId(id));

        return res != null ? _userMapper.MapToDTO(res) : null;    
    }
        
    public async Task<UserDTO?> UpdateUserAsync(Guid id, UserUpdateDTO dto)
    {
        var user = _userUpdateMapper.MapToEntity(dto);
        var res = await _userRepository.UpdateUserAsync(new UserId(id), user);

        return res != null ? _userMapper.MapToDTO(res) : null;       
    }

    public async Task<UserDTO?> DeleteUserAsync(Guid id)
    {
        var res = await _userRepository.DeleteUserAsync(new UserId(id));
        return res != null ? _userMapper.MapToDTO(res) : null;
    }

    public async Task<UserDTO?> RegisterUserAsync(UserRegistrationDTO dto) 
    {
        var existingUser = await _userRepository.GetUserByEmailAsync(dto.Email);
        if (existingUser != null)
        {
            _logger.LogInformation("User already exist: {Email}", dto.Email);

            throw new UserAlreadyExistsException();
        }       

        var user = _userRegistrationMapper.MapToEntity(dto);

        user.Id = UserId.NewId;
        user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        user.Salt = BCrypt.Net.BCrypt.GenerateSalt();

        var res = await _userRepository.RegisterUserAsync(user);

        return res != null ? _userMapper.MapToDTO(res) : null;
    }
}
