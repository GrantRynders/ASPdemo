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
using System.Globalization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ASPdemo.Pages;

public class PortfolioModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public string SearchTerm {  get; set; }
    public int SkipId { get; set; }

    [BindProperty]
    public List<PortfolioToken> PortfolioTokens { get; set; }

    [BindProperty]
    public TempPortfolio? TempPortfolio { get; set; }

    [FromQuery]
    public int SkipPrevious { get; set; }
    [MaxLength(42)]
    [BindProperty]
    public string? walletAddress { get; set; }
    [BindProperty]
    public decimal walletValue { get; set; }

    public User? currentUser { set; get; }
    public Portfolio? userPortfolio { get; set; }
    public string? userName { get; set; }
    public string? userEmail { get; set; }
    private readonly UserManager<User> _userManager;
    private ApplicationDbContext dbContext;

    public PortfolioModel(UserManager<User> userManager, ILogger<IndexModel> logger)
    {
        PortfolioTokens = new List<PortfolioToken>(); 

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
            if (currentUser.portfolio == null)
            {
                currentUser.portfolio = new Portfolio() {
                    WalletAddress = "",
                    PortfolioValue = 0,
                    UserId = currentUser.Id
                };
            }
            userName = currentUser.UserName;
            walletAddress = currentUser.portfolio.WalletAddress;
        }
        else
        {
            Console.WriteLine("Current User is null");
        }

        try
        {
            PortfolioTokens = dbContext.PortfolioTokens.Where(p => p.UserId == currentUser.Id).ToList();
        }
        catch
        {

        }
    }
    public async Task<IActionResult> OnPost(TempPortfolio tempPortfolio)
    {

        // try
        // {
        //     dbContext.PortfolioTokens.ExecuteDelete();
        //     dbContext.SaveChanges(); 
        // }
        // catch (Microsoft.Data.Sqlite.SqliteException) //catches if the users table does not exist yet
        // {
        //     Console.WriteLine("NO PORTFOLIOS TOKENS TABLE YET! CRAAAAAAAAAAAAAP!");
        //     return null;
        // }

        walletAddress = tempPortfolio.WalletAddress;
        Console.WriteLine("Wallet address: " + walletAddress);
        HttpClient client = new HttpClient();
        currentUser = await GetCurrentUser(dbContext);
        try
        {  
           
            //https://learn.microsoft.com/en-us/aspnet/core/razor-pages/?view=aspnetcore-8.0&tabs=visual-studio
            
            if (!ModelState.IsValid)
            {
                return Page(); 
            }
            if (currentUser != null)
            { 

                Console.WriteLine("currentuser != null");
                if (dbContext.Portfolios.Where(p => p.UserId == currentUser.Id).FirstOrDefault() == null)
                {
                    Console.WriteLine("Current user portfolio was null");
                    Portfolio portfolio = new Portfolio();
                    portfolio.WalletAddress = "0x3f5ce5fbfe3e9af3971dd833d26ba9b5c936f0be";
                    portfolio.PortfolioValue = 0;
                    portfolio.user = currentUser;
                    portfolio.UserId = currentUser.Id;
                    dbContext.Portfolios.Add(portfolio);
                    dbContext.SaveChanges();
                }
                else
                { 
                    Console.WriteLine("User portfolio is in fact not null");
                }
                userName = currentUser.UserName;
                dbContext.Portfolios.Where(p => p.UserId == currentUser.Id).FirstOrDefault().WalletAddress = walletAddress;
                Console.WriteLine("Wallet address: " + dbContext.Portfolios.Where(p => p.UserId == currentUser.Id).FirstOrDefault().WalletAddress);
                // if (tempPortfolio.WalletAddress != null && tempPortfolio.WalletAddress != string.Empty)
                // {
                //     currentUser.portfolio.WalletAddress = tempPortfolio.WalletAddress;
                // }
                // else
                // {
                //     Console.WriteLine("View data wallet address is null");
                // }
                if (dbContext.Portfolios.Where(p => p.UserId == currentUser.Id).FirstOrDefault().WalletAddress != string.Empty)
                {
                    var url = new UriBuilder("https://api.etherscan.io/api?module=account&action=balance&address=" + walletAddress + "&tag=latest&apikey=JVV4MYE725TUVIR7E6UNMYIZ6V2G67VXNT");
                    ViewData["Test"] = url;
                    string tokens = null;

                    Console.WriteLine(url.ToString());
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
                                dynamic result = results.result;
                                double test = Convert.ToDouble(result);
                                walletValue = Decimal.Parse(
                                        test.ToString(),
                                        NumberStyles.Any,
                                         CultureInfo.InvariantCulture);

                                var tokensAndBalances = await ApiCaller.getTokensAndBalances(tempPortfolio.WalletAddress); 
                                
                                if (tokensAndBalances != null && tokensAndBalances.Length > 0)
                                {
                                    dynamic tokenResults = JsonConvert.DeserializeObject(tokensAndBalances);

                                    dynamic rlTokenResults = tokenResults.result;
                                    dynamic tokenBalances = rlTokenResults.tokenBalances;

                                    var tokenCounter = 0;

                                    foreach (var token in tokenBalances)
                                    {
                                        if (tokenCounter <= 15)
                                        {
                                            string contractAddress = token.contractAddress;
                                            string tokenBalance = token.tokenBalance;

                                            string parsedBalance = ConvertBalance(tokenBalance);

                                            string tokenNameResult = await ApiCaller.getTokenNameFromContract(contractAddress);
                                            dynamic tokenNameConvert = JsonConvert.DeserializeObject<dynamic>(tokenNameResult);
                                            dynamic resultResponse = tokenNameConvert.result;

                                            string tokenName = ""; 

                                            try
                                            {
                                                tokenName = resultResponse[0].tokenName;
                                            }
                                            catch (Exception)
                                            {

                                            }


                                            if (tokenName != null && tokenName != string.Empty)
                                            {
                                                string currentUserId = currentUser.Id;

                                                var portfolioToken = new PortfolioToken();

                                                portfolioToken.TokenName = tokenName;
                                                portfolioToken.UserId = currentUserId;
                                                portfolioToken.TokenAmount = parsedBalance;

                                                dbContext.PortfolioTokens.Add(portfolioToken);
                                                dbContext.SaveChanges();

                                                tokenCounter++; 
                                            }

                                        }
                                    }
                                    PortfolioTokens = dbContext.PortfolioTokens.Where(p => p.UserId == currentUser.Id).ToList(); 

                                    return Page(); 

                                }
                            }

                            return Page(); 
                            
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
                else
                {
                    Console.WriteLine("Current User is null");
                }
            }
        }
        catch (Microsoft.Data.Sqlite.SqliteException) //catches if the portfolio table does not exist yet
        {
            Console.WriteLine("PORTFOLIO SQLITE EXCEPTION");
        }
        Console.WriteLine("POST finish");
        return RedirectToPage("./Portfolio"); 
    }

    public static string ConvertBalance(string balance)
    {
        string obj = new System.ComponentModel.Int128Converter().ConvertFromString(balance).ToString();
        return obj; 
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

public class TempPortfolio()
{
    public string WalletAddress { get; set; }
}