using ASPdemo.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace ASPdemo.Pages;

public class CurrenciesModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    [FromQuery]
    public int PageId { get; set; }

    [FromQuery]
    public int MaxId { get; set; }

    [BindProperty]
    public List<Currency> Currencies { get; set; }

    public CurrenciesModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public async Task OnGet()
    {
        Currencies = new List<Currency>(); 

        HttpClient client = new HttpClient();

        if (MaxId == 0)
        {
            MaxId = 10;
        }
        else
        {
            MaxId = MaxId + 10;
        }

        if (PageId == 0)
        {
            PageId = 1;
        }
        else
        {
            if (PageId == 1)
            {
                PageId = 0;
            }
            PageId = PageId + 10;
        }

        ViewData["MaxId"] = MaxId;
        ViewData["PageId"] = PageId;

        var url = new UriBuilder("https://pro-api.coinmarketcap.com/v2/cryptocurrency/info/" + MaxId + "/" + PageId);

        string tokens = await client.GetStringAsync(url.ToString()); 

        dynamic results = JsonConvert.DeserializeObject<dynamic>(tokens);

        foreach (dynamic result in results)
        {
            var currency = new Currency();

            var currencyId = result.currencyId; 
            var currencyName = result.currencyName;
            var currencySlug = result.categoryTitle;
            var description = result.description;

            currency.CurrencyId = currencyId; 
            currency.CurrencyName = currencyName;
            currency.Slug = currencySlug;
            currency.Description = description;

            Currencies.Add(currency);
        }
    }
}
