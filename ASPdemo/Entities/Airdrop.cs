using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ASPdemo.Entities;

[Table("Airdrops")]
public class Airdrop
{
    public int AirdropId { get; set; }
    public int CurrencyId { get; set; }
}