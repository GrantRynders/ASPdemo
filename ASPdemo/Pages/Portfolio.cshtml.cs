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

    [FromQuery]
    public int PageId { get; set; }

    [FromQuery]
    public int MaxId { get; set; }

    [BindProperty]
    public Portfolio userPortfolio { get; set; }

    [BindProperty]
    public Search? Search {  get; set; }
    public class InputModel
    {
        
    }
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

        var url = new UriBuilder("http://127.0.0.1:5220/Portfolios");
        ViewData["Test"] = url;
        string tokens = null;
        try
        {
            try
            {
                tokens = await client.GetStringAsync(url.ToString()); 
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("404 ERROR");
            }
            
            if (tokens != null)
            {
                dynamic results = JsonConvert.DeserializeObject<dynamic>(tokens);
                foreach (dynamic result in results)
                {
                    var portfolio = new Portfolio();

                    var portfolioId = result.PortfolioId;
                    var walletAddress = result.Address;
                    var portfolioValue = result.PortfolioValue;
                    var user = result.user;
                    var userId = result.UserId;

                    portfolio.PortfolioId = portfolioId;
                    portfolio.WalletAddress = walletAddress;
                    portfolio.PortfolioValue = portfolioValue;
                    portfolio.user = user;
                    portfolio.UserId = userId;
                    

                    userPortfolio = portfolio;
                }
            }
            else
            {
                Console.WriteLine("NO TOKENS TO DISPLAY");
            }
        }
        catch (Microsoft.Data.Sqlite.SqliteException) //catches if table is crapped
        {
            Console.WriteLine("TABLE DOES NOT EXIST");
        }
        catch (HttpRequestException)
        {
            Console.WriteLine("HTTP REQUEST EXCEPTION ON CATEGORIES POST");
        }
        catch (WebException)
        {
            Console.WriteLine("WEB EXCEPTION ON CATEGORIES POST");
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

                filteredRoles = new List<Role>();
                
                foreach (Role role in roles)
                {
                    if (role.Name.ToLower().Contains(Search.SearchTerm.ToLower())) //This should probably be done a lot better but this works for now
                    {
                        filteredRoles.Add(role); //Add to the filtered list
                    }
                }
                return Page(); 
            }
        }
        catch (Microsoft.Data.Sqlite.SqliteException) //catches if the users table does not exist yet
        {
            Console.WriteLine("SQLITE EXCEPTION");
        }
        return RedirectToPage("./Administration"); 
    }
    public async Task<Entities.User?> GetCurrentUser(ApplicationDbContext dbContext)
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