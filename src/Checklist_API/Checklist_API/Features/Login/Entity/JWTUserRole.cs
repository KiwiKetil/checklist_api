using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Checklist_API.Features.Login.Entity;

public readonly record struct JwtUserRoleId(Guid jwtUserRoleId)
{
    public static JwtUserRoleId NewId => new(Guid.NewGuid());
    public static JwtUserRoleId Empty => new(Guid.Empty);
}

public class JWTUserRole
{    
    public JwtUserRoleId Id { get; set; }

    public string? UserName { get; set; } // userid userid

    public int JwtRoleId { get; set; }
}
