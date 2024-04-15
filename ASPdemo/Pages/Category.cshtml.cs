using ASPdemo.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Drawing;

namespace ASPdemo.Pages
{
    public class CategoryModel : PageModel
    {
        [FromQuery]
        public string? CategoryId { get; set; }

		[BindProperty]
		public List<Currency>? Currencies { get; set; }

		[BindProperty]
		public string? CategoryName {  get; set; }

        public async Task<IActionResult> OnGet()
        {
            if (CategoryId == null)
            {
                return Redirect("./Categories");
            }

			try
			{
				Currencies = new List<Currency>();

				HttpClient client = new HttpClient();

				var url = new UriBuilder("http://127.0.0.1:5220/category/" + CategoryId);
				string tokens = await client.GetStringAsync(url.ToString());

				dynamic results = JsonConvert.DeserializeObject<dynamic>(tokens);

				var coins = results.coins;
	
				CategoryName = results.categoryName;

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

					Currencies.Add(currency);
				}
			}
			catch
			{
			} 

			return Page(); 
        }
    }
}
