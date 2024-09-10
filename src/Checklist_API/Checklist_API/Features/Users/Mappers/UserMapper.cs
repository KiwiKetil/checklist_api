using Checklist_API.Features.Common.Interfaces;
using Checklist_API.Features.Users.DTOs;
using Checklist_API.Features.Users.Entity;

namespace Checklist_API.Features.Users.Mappers;

public class UserMapper : IMapper<User, UserDTO>
{
    public UserDTO MapToDTO(User entity)
    {
        return new UserDTO(entity.FirstName, entity.LastName, entity.PhoneNumber, entity.Email, entity.DateCreated, entity.DateUpdated);
    }

    public User MapToEntity(UserDTO dto)
    {
        return new User()
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PhoneNumber = dto.Phonenumber,
            Email = dto.Email,
        };
    }
}
