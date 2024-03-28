using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
namespace ASPdemo.Entities;

[Table("Users")]
public class User : IdentityUser
{
    public override string Id { get; set; }
    [MaxLength(50)]
    public override string Email { get; set; }  
    [MaxLength(50)]
    public override string UserName { get; set; }
    
    public int UserId { get; set; }
    public int PortfolioId { get; set; }
    public int PermissionsLevel { get; set; } //should not be changeable (except by an admin perhaps), need to update the property to reflect this
    public List<Currency> followedCurrencies = new List<Currency>();
    public User()
    {
        PermissionsLevel = 0;
    }
}
