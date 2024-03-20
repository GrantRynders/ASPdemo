using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ASPdemo.Entities;

[Table("Currencies")]
public class Currency
{
    public int CurrencyId { get; set; }
    public int CategoryId { get; set; }
    [MaxLength(50)]
    public string CurrencyName { get; set; }
    public double ExchangeRate { get; set; } //PLACEHOLDER, a currency will not have one exchange rate that's silly

}