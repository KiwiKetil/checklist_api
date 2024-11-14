using Checklist_API.Features.Checklists.Entity;
using Checklist_API.Features.JWT.Entity;

namespace Checklist_API.Features.Users.Entity;

public readonly record struct UserId(Guid Value)
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

    public DateTime DateCreated { get; init; }

    public DateTime DateUpdated { get; set; }

    public virtual ICollection<CheckList> Checklists { get; set; } = [];
    public virtual ICollection<UserRole> UserRoles { get; set; } = [];
}
