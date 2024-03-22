using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPdemo.Pages;

public class CategoriesModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public CategoriesModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {

    }
}
