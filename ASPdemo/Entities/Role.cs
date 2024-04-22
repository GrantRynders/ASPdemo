using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
namespace ASPdemo.Entities;

[Table("Roles")]
public class Role : IdentityRole<string>
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public override string Id { get; set; }
    [MaxLength(50)]
    public override string Name { get; set; }  
    [MaxLength(50)]
    public override string NormalizedName { get; set; }
    public virtual List<User> Users { get; set; }
    public override string? ConcurrencyStamp { get; set; }
    public Role(){}
    public Role(string roleName)
    {
        this.Name = roleName;
        this.Id = Guid.NewGuid().ToString();
        Console.WriteLine("Role Id: " + this.Id);
        this.Users = new List<User>();
        this.NormalizedName = this.Name.Normalize();
    }
}