namespace ASPdemo.Pages;
using ASPdemo.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ASPdemo.Entities;
using Newtonsoft.Json;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Claims;

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

    public PersonalInfoModel()
    {
       
    }
    public async Task OnGet()
    {
        newUserName = (string?)ViewData["newUserName"];
        var currentUserId =  User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
        var currentUserName =  User.FindFirstValue(ClaimTypes.Name);
        userName = currentUserName;
        ViewData["userName"] = userName;
        var currentUserEmail =  User.FindFirstValue(ClaimTypes.Email);
        userEmail = currentUserEmail;
        ViewData["userEmail"] = userEmail;

    }
    public async Task<Entities.User> GetCurrentUser(ApplicationDbContext dbContext)
    {
        
        string? currentUserId = _userManager.GetUserId(User);
        Console.WriteLine("Current user id: ", currentUserId);
        if (currentUserId != null)
        {
            try
            {  
                User? currentUser = await _userManager.GetUserAsync(User);
                //User? currentUser = await dbContext.Users.FindAsync(currentUserId); //if we call this in the OnGet() method it has a stroke, perhaps the tables aren't ready at that point
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
        Console.WriteLine("POST BABEEEE");
        if (!ModelState.IsValid)
        {
            return Page(); 
        }
        newUserName = (string?)ViewData["newUserName"];
        if (newUserName != null)
        {
            await OnGet();
            var db = new ApplicationDbContext();
            User currentUser = await GetCurrentUser(db);
            await _userManager.SetUserNameAsync(currentUser, newUserName);
            
            return Page(); 
        }
        else
        {
            Console.WriteLine("NewUserName is null");
        }

        return RedirectToPage("./PersonalInfo"); 
    }
}
