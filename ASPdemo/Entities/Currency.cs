using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ASPdemo.Entities;

[Table("Currencies")]
public class Currency
{
    public int CurrencyId { get; set; }

    public int CategoryId { get; set; }
    public string? CMCId { get; set; }


    [MaxLength(50)]
    public string? CurrencyName { get; set; }
    
    [MaxLength(50)]
    public string? Slug { get; set; }
    [MaxLength(50)]
    public string? Symbol { get; set; }

    public double? PercentChange24Hr { get; set; }

    public string? Description { get; set; }

    public double? Price { get; set; }

    public double? Volume24 {  get; set; }
    
    public double? PercentChange1hr { get; set; }

    public double? PercentChange7d { get; set; }

    public double? MarketCap {  get; set; }

    public double? TotalSupply {  get; set; }
}