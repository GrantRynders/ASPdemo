using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
namespace ASPdemo.Entities;

[Table("Roles")]
public class Role : IdentityRole
{
    public override string Id { get; set; }
    [MaxLength(50)]
    public override string Name { get; set; }  
    [MaxLength(50)]
    public override string NormalizedName { get; set; }
    public virtual List<User> Users { get; set; }
    public Role()
    {
        
    }
}