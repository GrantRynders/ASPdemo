using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ASPdemo.Entities
{
    [Table("Users")]
    public class User : IdentityUser
    {
        [MaxLength(50)]
        public override string Email { get; set; }

        [MaxLength(50)]
        public override string UserName { get; set; }

        public int UserId { get; set; }

        public int PortfolioId { get; set; }
        public Portfolio Portfolio { get; set; }

        public List<Role> Roles { get; set; }

        public int PermissionsLevel { get; set; }

        public List<Currency> FollowedCurrencies { get; set; }

        public User()
        {
            PermissionsLevel = 0;
            FollowedCurrencies = new List<Currency>();
            Roles = new List<Role>(); // Initialize UserRoles collection
        }
    }
}




