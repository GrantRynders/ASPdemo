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
    
    [BindProperty]
    public InputModel Input { get; set; }
    public class InputModel
    {
        public string? newUserName { get; set; }
    }
    public string? userName { get; set; }
    public string? userEmail { get; set; }
    
    public string url { get; set; }

    private readonly UserManager<User> _userManager;
    private ApplicationDbContext dbContext;

    public PersonalInfoModel(UserManager<User> userManager)
    {
       _userManager = userManager;
       dbContext = new ApplicationDbContext();
    }
    public async Task OnGet()
    {
        var currentUserId =  User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
        var currentUserName =  User.FindFirstValue(ClaimTypes.Name);
        userName = currentUserName;
        ViewData["userName"] = userName;
        var currentUserEmail =  User.FindFirstValue(ClaimTypes.Email);
        userEmail = currentUserEmail;
        ViewData["userEmail"] = userEmail;

    }
    public async Task<Entities.User?> GetCurrentUser(ApplicationDbContext dbContext)
    {
        
        // string? currentUserId = _userManager.GetUserId(User);
        // Console.WriteLine("Current user id: ", currentUserId);
        try
        {  
            User? currentUser = await _userManager.GetUserAsync(User);
            Console.WriteLine("Current user id: " + currentUser.Id);
            //User? currentUser = await dbContext.Users.FindAsync(currentUserId); //if we call this in the OnGet() method it has a stroke, perhaps the tables aren't ready at that point
            return currentUser;
        }
        catch (Microsoft.Data.Sqlite.SqliteException) //catches if the users table does not exist yet
        {
            Console.WriteLine("NO USERS TABLE YET! CRAAAAAAAAAAAAAP!");
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
        if (Input.newUserName != null)
        {
            Console.WriteLine("New User Name: " + Input.newUserName);
            await OnGet();
            User? currentUser = await GetCurrentUser(dbContext);
            if (currentUser != null)
            {
                await _userManager.SetUserNameAsync(currentUser, Input.newUserName);
                Console.WriteLine("Current UserName: " + currentUser.UserName);
                await dbContext.SaveChangesAsync();
            }
            
            return Page(); 
        }
        else
        {
            Console.WriteLine("NewUserName is null");
        }

        return RedirectToPage("./PersonalInfo"); 
    }
}
