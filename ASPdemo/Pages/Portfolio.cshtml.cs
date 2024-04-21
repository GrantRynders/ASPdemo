using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ASPdemo.Entities;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Configuration;
using ASPdemo.Database;
using System.Net;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ASPdemo.Pages;

public class PortfolioModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public string SearchTerm {  get; set; }
    public int SkipId { get; set; }

    [FromQuery]
    public int SkipPrevious { get; set; }
    [MaxLength(42)]
    public string? walletAddress { get; set; }
    public double walletValue { get; set; }

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
        currentUser = await GetCurrentUser(dbContext);
        if (currentUser != null)
        {
            userPortfolio = currentUser.portfolio; 
            userName = currentUser.UserName;
        }
        else
        {
            Console.WriteLine("Current User is null");
        }
        if (userPortfolio.WalletAddress != null)
        {
            var url = new UriBuilder("https://api.etherscan.io/api?module=account&action=balance&address=" + walletAddress + "&tag=latest&apikey=JVV4MYE725TUVIR7E6UNMYIZ6V2G67VXNT");
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
                if (tokens != null && tokens.Length > 0)
                {
                    dynamic results = JsonConvert.DeserializeObject<dynamic>(tokens);
                    if (results != null)
                    {
                        foreach (dynamic result in results)
                        {
                            var value = result.result;
                            walletValue = double.Parse(value);
                            Console.WriteLine("Wallet value: " + walletValue);
                        }
                        ViewData["value"] = walletValue; 
                    }
                    
                }   
            else
            {
                Console.WriteLine("NO TOKENS FOR PORTFOLIO TO DISPLAY");
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
            ViewData["address"] = walletAddress;
            userPortfolio = new Portfolio();
            HttpClient client = new HttpClient();
            currentUser = await GetCurrentUser(dbContext);
            if (currentUser != null)
            {
                userPortfolio = currentUser.portfolio; 
                userName = currentUser.UserName;
                if (walletAddress != null)
                {
                    userPortfolio.WalletAddress = walletAddress;
                }
            }
            else
            {
                Console.WriteLine("Current User is null");
            }
            if (userPortfolio.WalletAddress != null)
            {
                var url = new UriBuilder("https://api.etherscan.io/api?module=account&action=balance&address=" + walletAddress + "&tag=latest&apikey=JVV4MYE725TUVIR7E6UNMYIZ6V2G67VXNT");
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
                    if (tokens != null && tokens.Length > 0)
                    {
                        dynamic results = JsonConvert.DeserializeObject<dynamic>(tokens);
                        if (results != null)
                        {
                            foreach (dynamic result in results)
                            {
                                var value = result.result;
                                walletValue = double.Parse(value);
                                Console.WriteLine("Wallet value: " + walletValue);
                            }
                            ViewData["value"] = walletValue; 
                        }
                        
                    }   
                    else
                    {
                        Console.WriteLine("NO TOKENS FOR PORTFOLIO TO DISPLAY");
                    }
                }
                catch (HttpRequestException)
                {
                    Console.WriteLine("HTTP REQUEST EXCEPTION ON PORTFOLIO POST");
                }
                catch (WebException)
                {
                    Console.WriteLine("WEB EXCEPTION ON PORTFOLIO POST");
                }
            }
        }
        catch (Microsoft.Data.Sqlite.SqliteException) //catches if the portfolio table does not exist yet
        {
            Console.WriteLine("PORTFOLIO SQLITE EXCEPTION");
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