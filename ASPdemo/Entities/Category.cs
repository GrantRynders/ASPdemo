using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ASPdemo.Entities;

[Table("Categories")]
public class Category
{
    [Key]
    public int CategoryId { get; set; }
    public string CMCCategoryId { get; set; }
    [MaxLength(50)]
    public string CategoryName { get; set; }
    [MaxLength(50)]
    public string CategoryTitle { get; set; }
    [MaxLength(100)]
    public string Description { get; set; }
    public int NumTokens { get; set; }
    public string AvgPriceChange { get; set; }
    public string MarketCap { get; set; }
    public string MarketCapChange { get; set; }
    public string Volume { get; set; }
    public string VolumeChange { get; set; }
    public double LastUpdated { get; set; }
    public List<Currency> Coins { get; set; }
}