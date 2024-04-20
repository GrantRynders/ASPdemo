using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using ASPdemo.Entities;
using Newtonsoft.Json;
using ASPdemo.Database;

namespace ASPdemo.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }
     
	[FromQuery]
	public int SkipPrevious { get; set; }


	[FromQuery]
	public int SkipId { get; set; }
    [FromQuery]
    public int SkipPrevious { get; set; }

	[BindProperty]
    public List<Currency> Currency { get; set; } 

    public async Task OnGet()
    {
        Currency = new List<Currency>(); 

        HttpClient client = new HttpClient();

        ViewData["SkipId"] = SkipId;


        var url = new UriBuilder("http://127.0.0.1:5220/listings/"+SkipId); //returns 500 error, tried listings/latest with no parameters and returned a 404
        //categories seems to return fine, so we might be calling this endpoint wrong
        // it would be better I think to use /quotes than /listings if we want the user to be able to query specific currencies
        try
            {
            string tokens = await client.GetStringAsync(url.ToString());

            dynamic results = JsonConvert.DeserializeObject<dynamic>(tokens);
            if (tokens != null)
            {
                foreach (dynamic result in results)
                {
                    var currencyName = result.currencyName; 
                    var currencyId = result.currencyId;
                    var slug = result.slug;
                    var symbol = result.symbol; 
                    var percentChange24hr = result.percentChange24Hr;
                    var price = result.price; 
                    var percentChange1hr = result.percentChange1hr;
                    var percentChange7d = result.percentChange7d;
                    var marketCap = result.marketCap;
                    var totalSupply = result.totalSupply; 

                    Currency currency = new Currency();

                    currency.CurrencyId = currencyId;
                    currency.Slug = slug;
                    currency.CurrencyName = currencyName;
                    currency.Price = price;
                    currency.Symbol = symbol; 
                    currency.PercentChange24Hr = percentChange24hr;
                    currency.MarketCap = marketCap;
                    currency.PercentChange7d = percentChange7d;
                    currency.PercentChange1hr = percentChange1hr;
                    currency.TotalSupply = totalSupply;

                    Currency.Add(currency);
                }
            }
            else
            {
                Console.WriteLine("NO TOKENS TO DISPLAY");
            }
        }
        catch (HttpRequestException)
        {
            Console.WriteLine("HTTP REQUEST EXCEPTION ON INDEX");
        }
    }



} 