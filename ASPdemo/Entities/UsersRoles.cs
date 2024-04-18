using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ASPdemo.Entities
{
    [Table("UserRoles")] // Adjust table name if needed
    public class UserRole : IdentityUserRole<string>
    {
        [Key]
        public int UserRoleId { get; set; } // Renamed primary key for clarity

        // Foreign key to User
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        // Foreign key to Role
        [ForeignKey(nameof(RoleId))]
        public IdentityRole Role { get; set; }
    }
}

