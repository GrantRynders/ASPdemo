using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ASPdemo.Entities;

[Table("Users")]
public class User
{
    public int UserId { get; set; }

    public int PortfolioId { get; set; }
    public int PermissionsLevel { get; set; } //should not be changeable (except by an admin perhaps), need to update the property to reflect this
    [MaxLength(50)]
    public string FirstName { get; set; }
    [MaxLength(50)]
    public string LastName { get; set; }
    [MaxLength(15)]
    public string UserName { get; set; }
    [MaxLength(50)]
    public string Email { get; set; } 
    public List<Currency> followedCurrencies = new List<Currency>();
    public User()
    {
        PermissionsLevel = 0;
    }
}
