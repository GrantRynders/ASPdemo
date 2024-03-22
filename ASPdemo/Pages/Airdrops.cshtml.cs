using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPdemo.Pages;

public class AirdropsModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public AirdropsModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {

    }
}
