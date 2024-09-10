namespace Checklist_API.Features.Users.DTOs;

public record UserDTO
(
    string FirstName,
    string LastName,
    string Phonenumber,
    string Email,
    DateTime DateCreated,
    DateTime DateUpdated
);