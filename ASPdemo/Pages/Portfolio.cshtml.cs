using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ASPdemo.Entities;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;

namespace ASPdemo.Pages
{
    public class PortfolioModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        [BindProperty]
        public string CurrencyName { get; set; }

        public PortfolioModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAddCurrencyAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return Unauthorized();
            }

            if (string.IsNullOrEmpty(CurrencyName))
            {
                ModelState.AddModelError(string.Empty, "Currency name is required.");
                return Page();
            }

            // Call CoinMarketCap API to get currency details
            var apiKey = "YOUR_COINMARKETCAP_API_KEY";
            var apiUrl = $"https://pro-api.coinmarketcap.com/v1/cryptocurrency/info?symbol={CurrencyName}";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", apiKey);

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<CoinMarketCapApiResponse>(json);

                    // Assuming 'CurrencyDetails' is a model representing currency details
                    var currencyDetails = apiResponse.Data.Values.FirstOrDefault();

                    if (currencyDetails != null)
                    {
                        // Create a new Currency object and add it to the user's portfolio
                        var newCurrency = new Currency
                        {
                            CurrencyName = currencyDetails.Name,
                            Symbol = currencyDetails.Symbol,
                            Price = currencyDetails.Quote.USD.Price,
                            PercentChange24Hr = currencyDetails.Quote.USD.PercentChange24H
                            // Add other properties here if needed
                        };

                        if (currentUser.Portfolio == null)
                        {
                            currentUser.Portfolio = new Portfolio();
                        }

                        currentUser.Portfolio.currencies.Add(newCurrency);

                        await _userManager.UpdateAsync(currentUser);

                        return RedirectToPage("./Portfolio");
                    }
                }
            }

            ModelState.AddModelError(string.Empty, "Failed to add currency. Please try again.");
            return Page();
        }

        public async Task<IActionResult> OnPostRemoveCurrencyAsync(string currencyName)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null || currentUser.Portfolio == null)
            {
                return NotFound();
            }

            var currencyToRemove = currentUser.Portfolio.currencies.FirstOrDefault(c => c.CurrencyName == currencyName);

            if (currencyToRemove != null)
            {
                currentUser.Portfolio.currencies.Remove(currencyToRemove);
                await _userManager.UpdateAsync(currentUser);
            }

            return RedirectToPage("./Portfolio");
        }
    }
}

