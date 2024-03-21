using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ASPdemo.Entities;

[Table("Airdrops")]
public class Airdrop
{
    public int AirdropId { get; set; }
    public int CurrencyId { get; set; }
    public string Status { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public double TotalPrize { get; set; }
    public double WinnerCount { get; set; }
    public string Link { get; set; }
}