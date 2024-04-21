using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPdemo.Entities
{
    [Table("PortfolioToken")]
    public class PortfolioToken
    {
        [Key]
        public int PortfolioTokenId { get; set; }
        
        public string UserId { get; set; }

        public string TokenAmount { get; set; }

        public string TokenName { get; set; }
    }
}
