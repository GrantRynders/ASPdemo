using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
namespace ASPdemo.Entities;

[Table("Roles")]
public class Admin : IdentityRole
{

    public override string Id { get; set; }
    public override string Name { get; set; }
    public override string NormalizedName { get; set; }
    public List<User> Users { get; set; }
    public Admin()
    {
        this.Name = "Admin";
        this.NormalizedName = "ADMIN";
    }
}