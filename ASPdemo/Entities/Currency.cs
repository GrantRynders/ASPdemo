using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ASPdemo.Entities;

[Table("Currencies")]
public class Currency
{
    public int CurrencyId { get; set; }

    public int CategoryId { get; set; }

    [MaxLength(50)]
    public string? CurrencyName { get; set; }
    
    [MaxLength(50)]
    public string? Slug { get; set; }
    [MaxLength(50)]
    public string? Symbol { get; set; }

    public string? Description { get; set; }
}