namespace ASPdemo.Pages;
using ASPdemo.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ASPdemo.Entities;
using Newtonsoft.Json;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

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
        var currentUser = await GetCurrentUser(dbContext);
        if (currentUser != null)
        {
            //var currentUserId =  User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId
            //var currentUserName =  User.FindFirstValue(ClaimTypes.Name);
            var currentUserName = await _userManager.GetUserNameAsync(currentUser);
            userName = currentUserName;
            ViewData["userName"] = userName;
            var currentUserEmail = await _userManager.GetEmailAsync(currentUser);
            //var currentUserEmail =  User.FindFirstValue(ClaimTypes.Email);
            userEmail = currentUserEmail;
            ViewData["userEmail"] = userEmail;
        }
        

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
    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page(); 
        }
        if (Input.newUserName != null)
        {
            await OnGet();
            User? currentUser = await GetCurrentUser(dbContext);
            if (currentUser != null)
            {
                await _userManager.SetUserNameAsync(currentUser, Input.newUserName);
                //Console.WriteLine("Current UserName: " + currentUser.UserName);
                //await dbContext.Database.MigrateAsync(); //ensures that the table exist PLEASE WORK
                dbContext.Entry(currentUser).State = EntityState.Modified;
                //Console.WriteLine("UserManager: new name of currentUser: " + await _userManager.GetUserNameAsync(currentUser));
                //await dbContext.SaveChangesAsync(); this fricking sucks, why does it never work?
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
