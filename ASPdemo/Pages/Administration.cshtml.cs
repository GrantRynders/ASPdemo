using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ASPdemo.Entities;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Configuration;
using ASPdemo.Database;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ASPdemo.Pages;

public class AdministrationModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public string SearchTerm {  get; set; }

    [BindProperty]
    public List<Role> roles { get; set; }

    public List<User> users { get; set; }
     [FromQuery]
    public int SkipId { get; set; }
    [FromQuery]
    public int SkipPrevious { get; set; }

    [BindProperty]
    public Search? Search {  get; set; }

    public List<Role>? filteredRoles { get; set; }
    public class InputModel
    {
        public string? newUserName { get; set; }
    }
    public string? userName { get; set; }
    public string? userEmail { get; set; }
    

    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly SignInManager<User> _signInManager;
    private ApplicationDbContext dbContext;

    public AdministrationModel(UserManager<User> userManager, ILogger<IndexModel> logger, SignInManager<User> signInManager, RoleManager<Role> roleManager)
    {
       _userManager = userManager;
       
       _signInManager = signInManager;
       dbContext = new ApplicationDbContext();
       _roleManager = roleManager;
       _logger = logger;
    }

    public async Task OnGet()
    {
        //await CreateRoleWithName("Admin");
        //await CreateRoleWithName("Cool Guys");
        

        roles = new List<Role>(); 

        HttpClient client = new HttpClient();
        ViewData["SkipId"] = SkipId;
        var url = new UriBuilder("http://127.0.0.1:5220/roles/" + SkipId);
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
                        var role = new Role();

                        var id = result.id;
                        var name = result.name;
                        var users = result.users;

                        role.Id = id;
                        role.Name = name;

                        foreach (var roleUser in users)
                        {
                            var user = new User();
                            user.Id = roleUser.Id;
                            user.UserName = roleUser.UserName;
                            user.Email = roleUser.Email;

                            users.Add(user);
                        }
                        roles.Add(role);
                    }
                    Console.WriteLine("ROLES COUNT : " + roles.Count());
                    ViewData["users"] = users; 
                }
                
            }
            else
            {
                Console.WriteLine("NO TOKENS FOR ROLES TO DISPLAY");
            }
        }
        catch (Microsoft.Data.Sqlite.SqliteException) //catches if table is crapped
        {
            Console.WriteLine("TABLE DOES NOT EXIST");
        }
        catch (HttpRequestException)
        {
            Console.WriteLine("HTTP REQUEST EXCEPTION ON ROLES GET");
        }
        catch (WebException)
        {
            Console.WriteLine("WEB EXCEPTION ON ROLES GET");
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
    public async Task CreateRoleWithName(string roleName)
    {
        if (await _roleManager.RoleExistsAsync(roleName) == false)
        {
            Role newRole = new Role(roleName);
            Console.WriteLine(roleName + "'s ID: " + newRole.Id);
            Boolean done = false;
            while (done == false)
            {
                if (await _roleManager.FindByIdAsync(newRole.Id) != null) //this makes sure we don't make duplicate IDs
                {
                    Console.WriteLine("Gabagool");
                    newRole.Id = Guid.NewGuid().ToString();
                    done = false;
                }
                else
                {
                    done = true;
                }
            }
            Console.WriteLine(newRole.Name);
            dbContext.Update(newRole);
            dbContext.Entry(newRole).State = EntityState.Modified;
            //await dbContext.SaveChangesAsync();

            //await _roleManager.CreateAsync(newRole);
        }
        else
        {
            Console.WriteLine("Role with name '" + roleName + "' already exists");
        }
    }
    public async Task JoinRole(Role role)
    {
        Console.WriteLine("Join role");
        if (User.IsInRole(role.Name) == false)
        {
            User? user = await GetCurrentUser(dbContext);
            if (user != null)
            {
                role.Users.Add(user);
                dbContext.Update(role);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
