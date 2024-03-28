using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ASPdemo.Entities;

[Table("CurrenciesPortfolios")]
public class CurrenciesPortfolios
{
    [Key]
    public int CurrenciesPortfoliosId { get; set; }
    public int CurrencyId { get; set; }
    public int PortfolioId { get; set; }
}