using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASPdemo.Pages;

public class PersonalInfoModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public PersonalInfoModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {

    }
}
