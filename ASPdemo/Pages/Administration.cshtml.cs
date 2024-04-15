using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ASPdemo.Entities;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Configuration;

namespace ASPdemo.Pages;

public class AdministrationModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public string SearchTerm {  get; set; }

    [FromQuery]
    public int PageId { get; set; }

    [FromQuery]
    public int MaxId { get; set; }

    [BindProperty]
    public List<Role> roles { get; set; }

    [BindProperty]
    public Search? Search {  get; set; }

    public List<Role>? filteredRoles { get; set; }

    public AdministrationModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public async Task OnGet()
    {
        roles = new List<Role>(); 

        HttpClient client = new HttpClient();

        if (MaxId == 0)
        {
            MaxId = 10;
        }
        else
        {
            MaxId = MaxId + 10;
        }

        if (PageId == 0)
        {
            PageId = 1;
        }
        else
        {
            if (PageId == 1)
            {
                PageId = 0;
            }
            PageId = PageId + 10;
        }

        ViewData["MaxId"] = MaxId;
        ViewData["PageId"] = PageId;

        var url = new UriBuilder("http://127.0.0.1:5220/roles/" + MaxId + "/" + PageId);
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
                    var role = new Role();

                    var id = result.id;
                    var name = result.name;
                    var users = result.users;

                    role.Id = id;
                    role.Name = name;
                    role.Users = users;

                    roles.Add(role);
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

        
    }
    public async Task<IActionResult> OnPost()
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

        return RedirectToPage("./Administration"); 
    }
    public User CreateUser()
    {
        try
        {
            return Activator.CreateInstance<User>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(User)}'. " +
                $"Ensure that '{nameof(User)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        }
    }
}
