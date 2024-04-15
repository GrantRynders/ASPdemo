namespace ASPdemo.Pages;
using ASPdemo.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ASPdemo.Entities;

public class LayoutModel : PageModel
{
    public string? userName { get; set; }

    private readonly UserManager<User> _userManager;
    private ApplicationDbContext dbContext;

    public LayoutModel(UserManager<User> userManager)
    {
       _userManager = userManager;
       dbContext = new ApplicationDbContext();
    }
    public async Task OnGet()
    {
        var currentUser = await GetCurrentUser(dbContext);
        if (currentUser != null)
        {
            var currentUserName = await _userManager.GetUserNameAsync(currentUser);
            userName = currentUserName;
            ViewData["layoutUserName"] = userName;
        }
    }
    public async Task<Entities.User?> GetCurrentUser(ApplicationDbContext dbContext)
    {
        
        // string? currentUserId = _userManager.GetUserId(User);
        // Console.WriteLine("Current user id: ", currentUserId);
        try
        {  
            User? currentUser = await _userManager.GetUserAsync(User);
            Console.WriteLine("Layout Username: " + currentUser.UserName);
            return currentUser;
        }
        catch (Microsoft.Data.Sqlite.SqliteException) //catches if the users table does not exist yet
        {
            Console.WriteLine("NO USERS TABLE YET! CRAAAAAAAAAAAAAP!");
            return null;
        }
    }
}