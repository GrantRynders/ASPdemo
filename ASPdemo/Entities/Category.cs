using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ASPdemo.Entities;

[Table("Categories")]
public class Category
{
    public int CategoryId { get; set; }
    [MaxLength(50)]
    public string CategoryName { get; set; }
}