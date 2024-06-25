namespace Checklist_API.Features.Users.Service.Interfaces;

public interface IUserService
{
    Task<ICollection<UserDTO>> GetAllAsync();
    Task<UserDTO> GetByIdAsync(Guid UserId);
    Task<UserDTO> UpdateAsync(Guid id, UserDTO dto);
    Task<UserDTO> DeleteAsync(Guid id);
    Task<UserDTO> RegisterUserAsync(UserRegisterDTO dto);
}
