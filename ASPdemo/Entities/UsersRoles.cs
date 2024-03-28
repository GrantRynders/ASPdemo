using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
namespace ASPdemo.Entities;

[Table("UsersRoles")]
public class UsersRoles
{
    public int UsersRolesId { get; set; }
    [MaxLength(50)]
    public string UserId { get; set; }
    [MaxLength(50)]
    public string RoleId { get; set; }
    public User user { get; set; }
    public IdentityRole role { get; set; }
    
}