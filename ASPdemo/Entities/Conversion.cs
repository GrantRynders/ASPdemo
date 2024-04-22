using System.ComponentModel.DataAnnotations;

namespace ASPdemo.Entities
{
    [System.ComponentModel.DataAnnotations.Schema.Table("Conversions")]
    public class Conversion
    {
        [Key]
        public int ConversionId { get; set; }

        public string? Pair1 { get; set; }

        public string? Pair2 { get; set; }

        public DateTime CreatedOn { get; set; }

        public double? PercentChange24Hr { get; set; }

        public string? Description { get; set; }

        public double? Price { get; set; }

        public double? Volume24 { get; set; }

        public double? PercentChange1hr { get; set; }

        public double? PercentChange7d { get; set; }

        public double? MarketCap { get; set; }

        public double? TotalSupply { get; set; }

        public double? SecondPercentChange24Hr { get; set; }

        public string? SecondDescription { get; set; }

        public double? SecondPrice { get; set; }

        public double? SecondVolume24 { get; set; }

        public double? SecondPercentChange1hr { get; set; }

        public double? SecondPercentChange7d { get; set; }

        public double? SecondMarketCap { get; set; }

        public double? SecondTotalSupply { get; set; }
    }
}
