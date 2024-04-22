using System.ComponentModel.DataAnnotations.Schema;

namespace ASPdemo.Entities
{
    [Table("Portfolios")]
    public class Portfolio
    {
        public int PortfolioId { get; set; }
        public string WalletAddress { get; set; }
        public double? PortfolioValue { get; set; }
        public string UserId {  get; set; } //FK for User
        public User user { get; set; } 
    }
}
