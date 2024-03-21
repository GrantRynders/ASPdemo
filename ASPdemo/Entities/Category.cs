using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ASPdemo.Entities;

[Table("Categories")]
public class Category
{
    public int CategoryId { get; set; }
    [MaxLength(50)]
    public string CategoryName { get; set; }
    [MaxLength(50)]
    public string CategoryTitle { get; set; }
    [MaxLength(100)]
    public string Description { get; set; }
    public int NumTokens { get; set; }
    public double AvgPriceChange { get; set; }
    public double MarketCap { get; set; }
    public double MarketCapChange { get; set; }
    public double Volume { get; set; }
    public double VolumeChange { get; set; }
    public double LastUpdated { get; set; }
}