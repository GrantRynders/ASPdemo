using System.ComponentModel.DataAnnotations.Schema;

namespace ASPdemo.Entities
{
    [Table("portfolio")]
    public class Portfolio
    {
        public int PortfolioId { get; set; }
        public string WalletAddress { get; set; }
        public double PortfolioValue { get; set; }
        public int UserId {  get; set; }
    }
}
