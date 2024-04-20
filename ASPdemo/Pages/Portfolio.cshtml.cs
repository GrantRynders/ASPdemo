using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ASPdemo.Entities;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Configuration;
using ASPdemo.Database;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace ASPdemo.Pages;

public class PortfolioModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public string SearchTerm {  get; set; }
    public int SkipId { get; set; }

    [FromQuery]
    public int SkipPrevious { get; set; }

    [BindProperty]
    public Search? Search { get; set; }
    public User? currentUser { set; get; }
    public Portfolio? userPortfolio { get; set; }
    public string? userName { get; set; }
    public string? userEmail { get; set; }
    private readonly UserManager<User> _userManager;
    private ApplicationDbContext dbContext;

    public PortfolioModel(UserManager<User> userManager, ILogger<IndexModel> logger)
    {
       _userManager = userManager;
       dbContext = new ApplicationDbContext();
       _logger = logger;
    }

    public async Task OnGet()
    {
        userPortfolio = new Portfolio();
        HttpClient client = new HttpClient();
        try
        {
            currentUser = await GetCurrentUser(dbContext);
           
            if (currentUser != null)
            {
                userPortfolio = currentUser.portfolio; 
                userName = currentUser.UserName;
                if (userPortfolio != null)
                {
                    if (userPortfolio.currencies != null)
                    {
                        foreach (Currency? currency in userPortfolio.currencies)
                        {
                            if (currency.Price != null)
                            {
                                userPortfolio.PortfolioValue += currency.Price;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("UserPortfolio is null");
                    }
                }
            }
            else
            {
                Console.WriteLine("Current User is null");
            }
            
        }
        catch (Microsoft.Data.Sqlite.SqliteException) //catches if table is crapped
        {
            Console.WriteLine("TABLE DOES NOT EXIST");
        }
        catch (HttpRequestException)
        {
            Console.WriteLine("HTTP REQUEST EXCEPTION ON PORTFOLIO GET");
        }
        catch (WebException)
        {
            Console.WriteLine("WEB EXCEPTION ON PORTFOLIO GET");
        }

        
    }
    public async Task<IActionResult> OnPost()
    {
        try
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

                return Page(); 
            }
        }
        catch (Microsoft.Data.Sqlite.SqliteException) //catches if the users table does not exist yet
        {
            Console.WriteLine("SQLITE EXCEPTION");
        }
        return RedirectToPage("./Portfolio"); 
    }
    public async Task<Entities.User?> GetCurrentUser(ApplicationDbContext db)
    {
        try
        {  
            User? currentUser = await _userManager.GetUserAsync(User);
            return currentUser;
        }
        catch (Microsoft.Data.Sqlite.SqliteException) //catches if the users table does not exist yet
        {
            Console.WriteLine("NO USERS TABLE YET! CRAAAAAAAAAAAAAP!");
            return null;
        }
    }
}