using ASPdemo.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ASPdemo.Pages;

public class CategoriesModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    [FromQuery]
    public string SearchTerm {  get; set; }

    [FromQuery]
    public int SkipId { get; set; }

    [BindProperty]
    public List<Category> Categories { get; set; }

    [BindProperty]
    public Search? Search {  get; set; }

    public List<Category>? filteredCategories { get; set; }

    public CategoriesModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public async Task OnGet()
    {
        Categories = new List<Category>(); 

        HttpClient client = new HttpClient();

        ViewData["SkipId"] = SkipId;

        var url = new UriBuilder("http://127.0.0.1:5220/categories/"+SkipId);
        try
        {
            string tokens = await client.GetStringAsync(url.ToString()); 
            if (tokens != null)
            {
                dynamic results = JsonConvert.DeserializeObject<dynamic>(tokens);
                foreach (dynamic result in results)
                {
                    var category = new Category();

                    var categoryId = result.categoryId; 
                    var categoryName = result.categoryName;
                    var categoryTitle = result.categoryTitle;
                    var description = result.description;
                    var numTokens = result.numTokens;
                    var volume = result.volume;
                    var avgPriceChange = result.avgPriceChange;
                    var marketCap = result.marketCap; 
                    var marketCapChange = result.marketCapChange;
                    var coins = result.coins;

                    var currencyDb = new List<Currency>(); 

                    foreach (var coin in coins)
                    {
                        var currency = new Currency();

                        currency.CurrencyId = coin.currencyId; 
                        currency.CurrencyName = coin.currencyName;
                        currency.CategoryId = coin.categoryId;
                        currency.Symbol = coin.symbol;
                        currency.Price = coin.price;
                        currency.PercentChange1hr = coin.percentChange1;
                        currency.PercentChange7d = coin.percentChange7d;
                        currency.PercentChange24Hr = coin.percentChange24Hr;
                        currency.MarketCap = coin.marketCap; 
                        currency.TotalSupply = coin.totalSupply;
                        currency.Slug = coin.slug;

                        currencyDb.Add(currency);
                    } 

                    category.CategoryId = categoryId;
                    category.MarketCap = marketCap; 
                    category.MarketCapChange = marketCapChange;
                    category.CategoryName = categoryName;
                    category.CategoryTitle = categoryTitle;
                    category.Description = description;
                    category.NumTokens = numTokens;
                    category.Volume = volume;
                    category.AvgPriceChange = avgPriceChange;
                    category.Coins = currencyDb;

                    Categories.Add(category);
                }
            }
            else
            {
                Console.WriteLine("NO TOKENS TO DISPLAY");
            }
        }
        catch (HttpRequestException)
        {
            Console.WriteLine("HTTP REQUEST EXCEPTION ON CATEGORIES GET");
        }
    }

    public async Task<IActionResult> OnPost(Search search)
    {
        //https://learn.microsoft.com/en-us/aspnet/core/razor-pages/?view=aspnetcore-8.0&tabs=visual-studio
        

        if (search != null)
        {
            filteredCategories = new List<Category>();

			HttpClient client = new HttpClient();

			var url = new UriBuilder("http://127.0.0.1:5220/categories/listall");
            try
            {
                string tokens = await client.GetStringAsync(url.ToString());

                dynamic results = JsonConvert.DeserializeObject<dynamic>(tokens);

                foreach (dynamic result in results)
                {
                    var category = new Category();
                    string categoryName = result.categoryName;

                    if (categoryName.ToLower().Contains(search.SearchTerm.ToLower()))
                    {
                        var categoryId = result.categoryId;
                        var categoryTitle = result.categoryTitle;
                        var description = result.description;
                        var numTokens = result.numTokens;
                        var volume = result.volume;
                        var avgPriceChange = result.avgPriceChange;
                        var marketCap = result.marketCap;
                        var marketCapChange = result.marketCapChange;
                        var coins = result.coins;

                        var currencyDb = new List<Currency>();

                        foreach (var coin in coins)
                        {
                            var currency = new Currency();

                            currency.CurrencyId = coin.currencyId;
                            currency.CurrencyName = coin.currencyName;
                            currency.CategoryId = coin.categoryId;
                            currency.Symbol = coin.symbol;
                            currency.Price = coin.price;
                            currency.PercentChange1hr = coin.percentChange1;
                            currency.PercentChange7d = coin.percentChange7d;
                            currency.PercentChange24Hr = coin.percentChange24Hr;
                            currency.MarketCap = coin.marketCap;
                            currency.TotalSupply = coin.totalSupply;
                            currency.Slug = coin.slug;

                            currencyDb.Add(currency);
                        }

                        category.CategoryId = categoryId;
                        category.MarketCap = marketCap;
                        category.MarketCapChange = marketCapChange;
                        category.CategoryName = categoryName;
                        category.CategoryTitle = categoryTitle;
                        category.Description = description;
                        category.NumTokens = numTokens;
                        category.Volume = volume;
                        category.AvgPriceChange = avgPriceChange;
                        category.Coins = currencyDb;

                        filteredCategories.Add(category);
                    }
                }
            }
            catch (HttpRequestException)
            {
                Console.WriteLine("HTTP REQUEST EXCEPTION ON CATEGORIES POST");
            }
			return Page(); 
        }

        return RedirectToPage("./Categories"); 
    }
}
 
public class Search
{
    [Required]
    public string? SearchTerm { get; set; }
}
