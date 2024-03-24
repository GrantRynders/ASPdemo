using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ASPdemo.Entities;

[Table("CurrenciesCategories")]
public class CurrenciesCategories
{
    [Key]
    public int CurrenciesCategoriesId { get; set; }
    public int CurrencyId { get; set; }
    public int CategoryId { get; set; }
}