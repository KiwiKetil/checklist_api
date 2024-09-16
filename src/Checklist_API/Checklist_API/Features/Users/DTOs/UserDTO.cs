namespace Checklist_API.Features.Users.DTOs;

public record UserDTO
(
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Email,
    DateTime DateCreated,
    DateTime DateUpdated
);