using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
namespace ASPdemo.Entities;

[Table("UsersRoles")]
public class UsersRoles : IdentityUserRole<string>
{
    [MaxLength(50)]
    public override required string UserId { get; set; }
    [MaxLength(50)]
    public override required string RoleId { get; set; }
    public User user { get; set; }
    public Role role { get; set; }
    
}