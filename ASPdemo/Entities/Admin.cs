using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ASPdemo.Entities;

[Table("Users")]
public class Admin : User
{
    //admin specific actions
    public Admin()
    {
        PermissionsLevel = 1;
    }
}