using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using ASPdemo.Entities;
using Newtonsoft.Json; 

namespace ASPdemo.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    [FromQuery]
    public int PageId { get; set; }

    [FromQuery]
    public int MaxId {  get; set; }

    [BindProperty]
    public List<Currency> Currency { get; set; }

    public async Task OnGet()
    {
        Currency = new List<Currency>(); 

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
            PageId =  PageId + 10; 
        }

        ViewData["MaxId"] = MaxId;
        ViewData["PageId"] = PageId; 


        var url = new UriBuilder("http://127.0.0.1:5220/listings/"+MaxId+"/"+PageId); //returns 500 error, tried listings/latest with no parameters and returned a 404
        //categories seems to return fine, so we might be calling this endpoint wrong

        string tokens = await client.GetStringAsync(url.ToString());

        dynamic results = JsonConvert.DeserializeObject<dynamic>(tokens);

        foreach (dynamic result in results)
        {
            var currencyName = result.currencyName; 
            var currencyId = result.currencyId;
            var slug = result.slug;

            Currency currency = new Currency();

            currency.CurrencyId = currencyId;
            currency.Slug = slug;
            currency.CurrencyName = currencyName;

            Currency.Add(currency);
        }
    }
}
