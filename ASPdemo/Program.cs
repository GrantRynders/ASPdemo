using System;
using System.Net;
using System.Web;
using ASPdemo;
using ASPdemo.Database;
using ASPdemo.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;
using Quartz;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddIdentity<User, Role>(options => options.SignIn.RequireConfirmedAccount = true)
// .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
// builder.Services.AddHttpContextAccessor();
// builder.Services.TryAddScoped<IUserValidator<User>, UserValidator<User>>();
// builder.Services.TryAddScoped<IPasswordValidator<User>, PasswordValidator<User>>();
// builder.Services.TryAddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
// builder.Services.TryAddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
// builder.Services.TryAddScoped<IRoleValidator<Role>, RoleValidator<Role>>();
// // No interface for the error describer so we can add errors without rev'ing the interface
// builder.Services.TryAddScoped<IdentityErrorDescriber>();
// builder.Services.TryAddScoped<ISecurityStampValidator, SecurityStampValidator<User>>();
// builder.Services.TryAddScoped<ITwoFactorSecurityStampValidator, TwoFactorSecurityStampValidator<User>>();
// builder.Services.TryAddScoped<IUserClaimsPrincipalFactory<User>, UserClaimsPrincipalFactory<User, Role>>();
//builder.Services.TryAddScoped<UserManager<User>>();
// builder.Services.TryAddScoped<SignInManager<User>>();
// builder.Services.TryAddScoped<RoleManager<Role>>();
//builder.Services.TryAddScoped<IUserEmailStore<User>>();


//CREATE SQLITE DB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite("Data Source=Crypto.db");
});

builder.Services.AddIdentity<User, Role>(options => options.SignIn.RequireConfirmedAccount = true)
.AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders().AddDefaultUI();

builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey("CryptoJob");
    q.AddJob<CryptoJob>(ops => ops.WithIdentity(jobKey));

    q.AddTrigger(opts => opts.ForJob(jobKey).WithIdentity("CryptoJob-trigger")
    .WithCronSchedule("0 0/5 * * * ?"));
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

//############## CRUD OPS #######################

//get all users with page
app.MapGet("/users/{maxId}/{pageId}", async (ApplicationDbContext dbContext) => 
{
    return await dbContext.Users.ToListAsync(); 
});

app.MapGet("/add_categories", async (ApplicationDbContext dbContext) =>
{
    /** var result = ApiCaller.getCategories().Result;
     dynamic categories = JsonConvert.DeserializeObject<dynamic>(result).data;

     var list = new List<dynamic>();

     foreach (dynamic category in categories)
     {
         try
         {
             string categoryId = category.id;
             string categoryName = category.name;
             string title = category.title;
             string description = category.description;
             int num_tokens = category.num_tokens;
             string market_cap = category.market_cap;
             string market_cap_change = category.market_cap_change;
             string volume = category.volume; 

             var categoryDb = new Category(); 
             categoryDb.Description = description;
             categoryDb.CMCCategoryId = categoryId;
             categoryDb.CategoryTitle = title; 
             categoryDb.CategoryName = categoryName;
             categoryDb.NumTokens = num_tokens; 
             categoryDb.MarketCap = market_cap;
             if (market_cap_change != null)
             {
                 categoryDb.MarketCapChange = market_cap_change;
             }
             else
             {
                 categoryDb.MarketCapChange = "0";
             }
             categoryDb.Volume = volume;
             categoryDb.AvgPriceChange = "0"; 
             categoryDb.VolumeChange = "0";
             categoryDb.LastUpdated = 0; 

             dbContext.Categories.Add(categoryDb);
             dbContext.SaveChanges(); 
         }
         catch
         {

         }
     }**/
    /**  string response = await ApiCaller.getCategoryWithCoins("605e2ce9d41eae1066535f7c");
      Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(response);
      **/

    //var categories = dbContext.Categories.Include(p => p.Coins).ToList();
    // return categories;

    var categories = dbContext.Categories.Where(p => p.Coins.Count == 0).ToList(); 

    var list = new List<string>(); 

    foreach (var category in categories)
    {
        var cmcId = category.CMCCategoryId;

        string response = await ApiCaller.getCategoryWithCoins(cmcId);
        Root root = new Root();
        if (response != null)
        {
            root = JsonConvert.DeserializeObject<Root>(response);
            var coins = new List<Currency>();

            foreach (var coin in root.data.coins)
            {
                var random = new Random();
                int id = random.Next();

                var currency = new Currency();

                currency.CurrencyId = id; 
                currency.CategoryId = category.CategoryId;
                currency.CurrencyName = coin.name;
                currency.TotalSupply = coin.total_supply;

                if (coin.quote.USD != null)
                {
                    currency.Price = coin.quote.USD.price;
                    currency.PercentChange1hr = coin.quote.USD.percent_change_1h;
                    currency.PercentChange7d = coin.quote.USD.percent_change_7d;
                    currency.MarketCap = coin.quote.USD.market_cap;
                    currency.PercentChange24Hr = coin.quote.USD.percent_change_24h; 
                }

                currency.Slug = coin.slug;
                currency.Symbol = coin.symbol;
                currency.Description = "fawefef";

                dbContext.Currencies.Add(currency);
                dbContext.SaveChanges();

                coins.Add(currency);

                Thread.Sleep(5000);
            }

            category.Coins = coins; 
            dbContext.Update(category); 
            dbContext.SaveChanges();
        }  
    }
}); 

app.MapGet("/listings/{skipId}", async (int skipId, ApplicationDbContext db) =>
{
	var listings = db.Currencies.Skip(skipId).Take(10).ToList();

	return listings;
});
app.MapGet("/roles/{skipId}", async (int skipId, ApplicationDbContext db) =>
{
    var roles = db.Roles.Include(p => p.Users).Skip(skipId).Take(10).ToList(); 
    return roles;
});

app.MapGet("/category/{categoryId}", async (int categoryId, ApplicationDbContext db) =>
{
    var categories = db.Categories.Include(p => p.Coins).Where(p => p.CategoryId == categoryId).FirstOrDefault();

    return categories;
});

app.MapGet("/categories/listall", async (ApplicationDbContext db) =>
{
    var categories = db.Categories.Include(p => p.Coins).ToList(); 

	return categories;
});

app.MapGet("/categories/{skipId}", async (int skipId, ApplicationDbContext db) =>
{
    var categories = db.Categories.Include(p => p.Coins).Skip(skipId).Take(10).ToList(); 

    return categories;
});

// REQUEST 8: get a user
app.MapGet("/users/{userId}", async (int userId, ApplicationDbContext dbContext) => 
await dbContext.Users.FindAsync(userId)
is User user
? Results.Ok(user)
: Results.NotFound());

// REQUEST 5: create a user
app.MapPost("/users", async (User user, ApplicationDbContext dbContext) => 
{
    dbContext.Users.Add(user);
    await dbContext.SaveChangesAsync(); 
    return Results.Created($"/users/{user.Id}", user);
});

// REQUEST 6: update a user

app.MapPut("/users/{Id}", async (string userId, User user, ApplicationDbContext dbContext) => 
{
    if (userId != user.Id)
    {
        return Results.BadRequest("UserId mismatch");
    }
    dbContext.Entry(user).State = EntityState.Modified;
    await dbContext.SaveChangesAsync();
    return Results.NoContent();
});

// REQUEST 7: delete a user

app.MapDelete("/users/{Id}", async (string id, ApplicationDbContext dbContext) => 
{
    dbContext.Users.Remove(new User { Id = id});
    //Console.WriteLine("Deleted user with ID: " + userId); DEBUG
    await dbContext.SaveChangesAsync();
});


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();