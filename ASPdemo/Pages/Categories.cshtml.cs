using ASPdemo.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json; 

namespace ASPdemo.Pages;

public class CategoriesModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    [FromQuery]
    public string SearchTerm {  get; set; }

    [FromQuery]
    public int PageId { get; set; }

    [FromQuery]
    public int MaxId { get; set; }

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

        var url = new UriBuilder("http://127.0.0.1:5220/categories/" + MaxId + "/" + PageId);
        ViewData["Test"] = url; 
        string tokens = await client.GetStringAsync(url.ToString()); 

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
            // temp: we need to get the list of coins from the JSON
            //var coins = result.coins

            category.CategoryId = categoryId; 
            category.CategoryName = categoryName;
            category.CategoryTitle = categoryTitle;
            category.Description = description;
            category.NumTokens = numTokens;
            category.Volume = volume;
            category.AvgPriceChange = avgPriceChange;
            //category.Coins = coins

            Categories.Add(category);
        }
    }

    public async Task<IActionResult> OnPost()
    {
        //https://learn.microsoft.com/en-us/aspnet/core/razor-pages/?view=aspnetcore-8.0&tabs=visual-studio
        
        if (!ModelState.IsValid)
        {
            return Page(); 
        }

        if (Search != null)
        {
            ViewData["SearchTerm"] = Search.SearchTerm; 

            await OnGet(); 

            filteredCategories = new List<Category>();

            foreach (Category category in Categories)
            {
                if (category.CategoryName.ToLower().Contains(Search.SearchTerm.ToLower())) //This should probably be done a lot better but this works for now
                {
                    filteredCategories.Add(category); //Add to the filtered list
                }
            }

            return Page(); 
        }

        return RedirectToPage("./Categories"); 
    }
}

public class Search
{
    public string? SearchTerm { get; set; }
}
