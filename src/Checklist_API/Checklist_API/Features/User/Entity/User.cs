using System.ComponentModel.DataAnnotations;

namespace Checklist_API.Features.User.Entity;

public readonly record struct UserId(Guid userId)
{
    public static UserId NewId => new(Guid.NewGuid());
    public static UserId Empty => new(Guid.Empty);
};

public class User
{    
    public UserId Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string HashedPassword { get; set; } = string.Empty;

    public string Salt { get; set; } = string.Empty;

    public DateTime Created { get; init; }

    public DateTime Updated { get; set; }
}
