using System.ComponentModel.DataAnnotations;

namespace Checklist_API.Features.Login.Entity;

public readonly record struct JwtRoleId(Guid jwtRoleId)
{
    public static JwtRoleId NewId => new(Guid.NewGuid());
    public static JwtRoleId Empty => new(Guid.Empty);
};

public class JWTRole
{
    [Key]
    public JwtRoleId Id { get; set; }
    public string RoleName { get; set; } = string.Empty;
}
