namespace ASPdemo
{
    public class Coin
    {
         // morning portfolio+todo
        public int id { get; set; }
        public string? name { get; set; }
        public string? symbol { get; set; }
        public string? slug { get; set; }
        public int? num_market_pairs { get; set; }
        public DateTime? date_added { get; set; }
        public List<string>? tags { get; set; }
        public long? max_supply { get; set; }
        public double? circulating_supply { get; set; }
        public double? total_supply { get; set; }
        public int? is_active { get; set; }
        public bool? infinite_supply { get; set; }
        public Platform? platform { get; set; }
        public int? cmc_rank { get; set; }
        public int? is_fiat { get; set; }
        public double? self_reported_circulating_supply { get; set; }
        public double? self_reported_market_cap { get; set; }
        public double? tvl_ratio { get; set; }
        public DateTime? last_updated { get; set; }
        public Quote? quote { get; set; }
    }

    public class Data
    {
        public string? id { get; set; }
        public string? name { get; set; }
        public string? title { get; set; }
        public string? description { get; set; }
        public int? num_tokens { get; set; }
        public DateTime? last_updated { get; set; }
        public double? avg_price_change { get; set; }
        public double? market_cap { get; set; }
        public double? market_cap_change { get; set; }
        public double? volume { get; set; }
        public double? volume_change { get; set; }
        public List<Coin>? coins { get; set; }
    }

    public class Platform
    {
        public int? id { get; set; }
        public string? name { get; set; }
        public string? symbol { get; set; }
        public string? slug { get; set; }
        public string? token_address { get; set; }
    }

    public class Quote
    {
        public USD? USD { get; set; }
    }

    public class Root
    {
        public Status? status { get; set; }
        public Data? data { get; set; }
    }

    public class Status
    {
        public DateTime? timestamp { get; set; }
        public int? error_code { get; set; }
        public object? error_message { get; set; }
        public int? elapsed { get; set; }
        public int? credit_count { get; set; }
        public object? notice { get; set; }
    }

    public class USD
    {
        public double? price { get; set; }
        public double? volume_24h { get; set; }
        public double? volume_change_24h { get; set; }
        public double? percent_change_1h { get; set; }
        public double? percent_change_24h { get; set; }
        public double? percent_change_7d { get; set; }
        public double? percent_change_30d { get; set; }
        public double? percent_change_60d { get; set; }
        public double? percent_change_90d { get; set; }
        public double? market_cap { get; set; }
        public double? market_cap_dominance { get; set; }
        public double? fully_diluted_market_cap { get; set; }
        public double? tvl { get; set; }
        public DateTime? last_updated { get; set; }
    }
}
