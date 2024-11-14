namespace Checklist_API.Features.Users.DTOs;

public record UserRegistrationDTO(
string FirstName,
string LastName,
string PhoneNumber,
string Email,
string Password
);
