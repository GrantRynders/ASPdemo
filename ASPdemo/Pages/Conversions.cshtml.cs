using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ASPdemo.Entities;
using Newtonsoft.Json;
using System.Net;
using ASPdemo.Database;
namespace ASPdemo.Pages;

public class ConversionsModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    [BindProperty]
    public List<Conversion> Conversions { get; set; }

    [BindProperty]
    public Search? search { get; set; }

    [BindProperty]
    public List<string> Pairs { get; set; }

    private readonly ApplicationDbContext _dbContext;

    public ConversionsModel(ApplicationDbContext dbContext, ILogger<IndexModel> logger)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<List<string>> GetPairs()
    {
        var list = new List<string>();

        try
        {
            HttpClient client = new HttpClient();

            UriBuilder url = new UriBuilder("http://127.0.0.1:5220/listings/all");

            string tokens = await client.GetStringAsync(url.ToString());

            dynamic results = JsonConvert.DeserializeObject<dynamic>(tokens);

            foreach (var coin in results)
            {
                string slug = coin.slug;
                list.Add(slug);
            }
        }
        catch (HttpRequestException)
        {
            Console.WriteLine("HTTP REQUEST EXCEPTION ON CONVERSIONS GET");
        }
        catch (WebException)
        {
            Console.WriteLine("WEB EXCEPTION ON CONVERSIONS GET");
        }

        return list;
    }

    public async Task<List<Conversion>> GetConversions()
    {
        var list = new List<Conversion>();

        HttpClient client = new HttpClient();

        var url = new UriBuilder("http://127.0.0.1:5220/conversions/all");

        try
        {
            string tokens = await client.GetStringAsync(url.ToString());

            dynamic result = JsonConvert.DeserializeObject(tokens);

            foreach (var conversion in result)
            {
                string pair1 = conversion.pair1;
                string pair2 = conversion.pair2;
                double? percentChange24hr = conversion.percentChange24Hr;
                double? price = conversion.price;
                double? percentChange1hr = conversion.percentChange1hr;
                double? percentChange7d = conversion.percentChange7d;
                double? marketCap = conversion.marketCap;
                double? totalSupply = conversion.totalSupply;
                double? volume = conversion.volume24;

                double? secondPrice = conversion.secondPrice;
                double? secondPercentChange24hr = conversion.secondPercentChange24Hr;
                double? secondPercentChange1hr = conversion.secondPercentChange1hr;
                double? secondPercentChange7d = conversion.secondPercentChange7d;
                double? secondMarketCap = conversion.secondMarketCap;
                double? secondTotalSupply = conversion.secondTotalSupply;
                double? secondVolume = conversion.secondVolume24;

                var todoTest = new Conversion();

                todoTest.Pair1 = pair1;
                todoTest.Pair2 = pair2;
                todoTest.PercentChange24Hr = percentChange24hr;
                todoTest.Price = price;
                todoTest.PercentChange1hr = percentChange1hr;
                todoTest.PercentChange7d = percentChange7d;
                todoTest.MarketCap = marketCap;
                todoTest.TotalSupply = totalSupply;
                todoTest.Volume24 = volume;

                todoTest.SecondPrice = secondPrice;
                todoTest.SecondPercentChange24Hr = secondPercentChange24hr;
                todoTest.SecondPercentChange1hr = secondPercentChange1hr;
                todoTest.SecondPercentChange7d = secondPercentChange7d;
                todoTest.SecondMarketCap = secondMarketCap;
                todoTest.SecondVolume24 = secondVolume;

                list.Add(todoTest);
            }

        }
        catch (Exception)
        {

        }

        return list;
    }

    public async Task OnGet()
    {
        Conversions = new List<Conversion>();
        Pairs = new List<string>();

        Pairs = GetPairs().Result;
        Conversions = GetConversions().Result;

        ViewData["Pairs"] = Pairs;
        ViewData["Conversions"] = Conversions;


        // https://jsonformatter.org/


    }

    public void ParseData(dynamic data1, dynamic data2)
    {
        string slug = data1.slug;
        double price = 0;
        double totalSupply = data1.total_supply;
        double marketCap = 0;
        double percentChange7d = 0;
        double percentChange24hr = 0;
        double percentChange1hr = 0;

        string secondSlug = data2.slug;
        double secondPrice = 0;
        double secondTotalSupply = 0;
        double secondPercentChange7d = 0;
        double secondPercentChange24hr = 0;
        double secondChange1hr = 0;
        double secondMarketCap = 0;

        if (data2.quote != null)
        {
            try
            {
                secondPrice = data2.quote.USD.price;
                secondMarketCap = data2.quote.USD.market_cap;
                secondPercentChange7d = data2.quote.USD.percent_change_7d;
                secondPercentChange24hr = data2.quote.USD.percent_change_24h;
                secondChange1hr = data2.quote.USD.percent_change_1h;
            }
            catch (Exception)
            {
            }
        }

        if (data1.quote != null)
        {
            try
            {
                price = data1.quote.USD.price;
                marketCap = data1.quote.USD.market_cap;
                percentChange7d = data1.quote.USD.percent_change_7d;
                percentChange24hr = data1.quote.USD.percent_change_24h;
                percentChange1hr = data1.quote.USD.percent_change_1h;
            }
            catch (Exception)
            {
            }

            try
            {
                Conversion conversion = new Conversion();

                conversion.Pair1 = slug;
                conversion.Pair2 = secondSlug;
                conversion.MarketCap = marketCap;
                conversion.TotalSupply = totalSupply;
                conversion.Price = price;
                conversion.PercentChange24Hr = percentChange24hr;
                conversion.PercentChange1hr = percentChange1hr;
                conversion.PercentChange7d = percentChange7d;
                conversion.Price = price;
                conversion.CreatedOn = DateTime.Now;

                conversion.SecondMarketCap = secondMarketCap;
                conversion.SecondPrice = secondPrice;
                conversion.SecondPercentChange24Hr = secondPercentChange24hr;
                conversion.SecondPercentChange7d = secondPercentChange7d;
                conversion.SecondPercentChange1hr = secondChange1hr;
                conversion.TotalSupply = secondTotalSupply;


                _dbContext.Conversions.Add(conversion);
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
            }
        }
    }


    public async Task<IActionResult> OnPost(Search search)
    {
        //https://learn.microsoft.com/en-us/aspnet/core/razor-pages/?view=aspnetcore-8.0&tabs=visual-studio

        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            string pair1 = search.SearchTerm;
            string pair2 = search.SecondTerm;

            string pairs = pair1 + "," + pair2;

            string response = await ApiCaller.GetLatestQuotesFromPairs(pairs);

            dynamic results = JsonConvert.DeserializeObject<dynamic>(response);
            dynamic data = results.data;

            var uri = new UriBuilder("http://127.0.0.1:5220/conversions/" + pair1 + "/" + pair2);
            var httpClient = new HttpClient();

            string pairIds = await httpClient.GetStringAsync(uri.ToString());

            string pairId1 = pairIds.Split(",")[0];
            string pairId2 = pairIds.Split(",")[1];

            var pair1Data = data[pairId1];
            var pair2Data = data[pairId2];

            ParseData(pair1Data, pair2Data);

            return Redirect("./Conversions");
        }
        catch (Exception)
        {
        }

        return Redirect("./Conversions");
    }
    public class Search
    {
        public string? SearchTerm { get; set; }

        public string? SecondTerm { get; set; }
    }

}
