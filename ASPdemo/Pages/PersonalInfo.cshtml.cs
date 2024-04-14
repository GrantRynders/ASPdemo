namespace ASPdemo.Pages;
using ASPdemo.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ASPdemo.Entities;
using Newtonsoft.Json;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class PersonalInfoModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    public string? userName { get; set; }
    public string? userEmail { get; set; }
    public string? newUserName { get; set; }
    public string url { get; set; }

    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IUserStore<User> _userStore;

    public PersonalInfoModel(ILogger<IndexModel> logger, UserManager<User> userManager, IUserStore<User> userStore, SignInManager<User> signInManager)
    {
        _logger = logger;
        _userManager = userManager;
        _userStore = userStore;
        _signInManager = signInManager;
    }
    public async Task OnGet()
    {
        var db = new ApplicationDbContext();
        User currentUser = await GetCurrentUser(db);
        if (currentUser != null)
        {
            userName = currentUser.UserName;
            ViewData["userName"] = userName;
            userEmail = currentUser.Email;
            ViewData["userEmail"] = userEmail;
        }
    }
    public async Task<Entities.User> GetCurrentUser(ApplicationDbContext dbContext)
    {
        string? currentUserId = _userManager.GetUserId(User);
        Console.WriteLine("Current user id: ", currentUserId);
        if (currentUserId != null)
        {
            try
            {  
                User? currentUser = await dbContext.Users.FindAsync(currentUserId); //if we call this in the OnGet() method it has a stroke, perhaps the tables aren't ready at that point
                return currentUser;
            }
            catch (Microsoft.Data.Sqlite.SqliteException ex) //catches if the users table does not exist yet
            {
                Console.WriteLine("NO USERS TABLE YET! CRAAAAAAAAAAAAAP!");
                return null;
            }
        }
        else
        {
            Console.WriteLine("Uh oh. Current User has no ID");
            return null;
        }
    }
    public async Task<IActionResult> OnPost()
    {
        //https://learn.microsoft.com/en-us/aspnet/core/razor-pages/?view=aspnetcore-8.0&tabs=visual-studio
        
        if (!ModelState.IsValid)
        {
            return Page(); 
        }

        if (newUserName != null)
        {
            OnGet();
            var db = new ApplicationDbContext();
            User currentUser = await GetCurrentUser(db);
            currentUser.UserName = newUserName;
            return Page(); 
        }
        else
        {
            Console.WriteLine("NewUserName is null");
        }

        return RedirectToPage("./Categories"); 
    }
}
