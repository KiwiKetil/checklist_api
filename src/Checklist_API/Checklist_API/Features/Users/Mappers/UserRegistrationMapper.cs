using Checklist_API.Features.Common.Interfaces;
using Checklist_API.Features.Users.DTOs;
using Checklist_API.Features.Users.Entity;

namespace Checklist_API.Features.Users.Mappers;

public class UserRegistrationMapper : IMapper<User, UserRegistrationDTO>
{
    public UserRegistrationDTO MapToDTO(User entity)
    {
        throw new NotImplementedException();
    }

    public User MapToEntity(UserRegistrationDTO dto)
    {
        var dtNow = DateTime.Now;

        return new User() 
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            DateCreated = dtNow,
            DateUpdated = dtNow,
        };
    }
}
