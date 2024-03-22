using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPdemo.Pages;

public class AdministrationModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public AdministrationModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {

    }
}
