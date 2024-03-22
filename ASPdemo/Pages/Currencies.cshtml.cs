using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPdemo.Pages;

public class CurrenciesModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public CurrenciesModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {

    }
}
