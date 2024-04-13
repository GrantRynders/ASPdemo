using ASPdemo.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ASPdemo.Entities;

namespace ASPdemo.Pages;

public class PersonalInfoModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    public string? userName { get; set; }
    public string? userEmail { get; set; }

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
    public void OnGet()
    {
        //User currentUser = GetCurrentUser();
        //userName = currentUser.UserName;
        //userEmail = currentUser.Email;
    }
    public Entities.User GetCurrentUser()
    {
        string? currentUserId = _userManager.GetUserId(User);
        if (currentUserId != null)
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            User? currentUser = dbContext.Users.Find(currentUserId); //if we call this in the OnGet() method it has a stroke, perhaps the tables aren't ready at that point
            return currentUser;
        }
        else
        {
            Console.WriteLine("Uh oh. Current User has no ID");
            return null;
        }
    }
}
