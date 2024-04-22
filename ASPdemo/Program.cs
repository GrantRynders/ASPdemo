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
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Configuration;
using static System.Formats.Asn1.AsnWriter;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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
.AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders().AddDefaultUI()
.AddUserStore<UserStore<User, Role, ApplicationDbContext, string>>()
.AddRoleStore<RoleStore<Role, ApplicationDbContext, string>>();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
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

app.UseCors(builder =>
{
    builder
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
}); 

app.MapGet("/fetch_progress/", async (ApplicationDbContext dbContext) =>
{
    var currencies = dbContext.Currencies.ToList();
    var categories = dbContext.Categories.ToList();

    var progressBar = new ProgressBar();

    progressBar.CurrenciesCount = currencies.Count; 
    progressBar.CategoriesCount = categories.Count;

    var progressBarJson = JsonConvert.SerializeObject(progressBar);

    return progressBarJson; 
});

//############## CRUD OPS #######################

app.MapGet("/conversions/{pair1}/{pair2}", async (string pair1, string pair2, ApplicationDbContext dbContext) =>
{
    var dbPair1 = dbContext.Currencies.Where(p => p.Slug == pair1).FirstOrDefault();
    var dbPair2 = dbContext.Currencies.Where(p => p.Slug == pair2).FirstOrDefault();

    var idPair1 = dbPair1.CMCId;
    var idPair2 = dbPair2.CMCId;

    return idPair1 + "," + idPair2;
});

app.MapGet("/conversions/all", async (ApplicationDbContext dbContext) => {
    var conversions = dbContext.Conversions.ToList();

    return conversions;
});

app.MapGet("/listings/all", async (ApplicationDbContext db) =>
{
    var all = db.Currencies.ToList();
    return all;
});
app.MapGet("/users", async (ApplicationDbContext db) =>
{
    var all = db.Users.Include(p => p.Roles).Include(p => p.portfolio).ToList();
    return all;
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
app.MapPost("/roles/{Name}", async (string Name, ApplicationDbContext db) =>
{
    Role role = new Role(Name);
    db.Roles.Add(role);
    await db.SaveChangesAsync(); 
    return Results.Created($"/roles/{Name}", role);
});
app.MapPut("/roles/{RoleId}/{UserId}", async (string RoleId, string UserId, ApplicationDbContext db) =>
{
    Role role = db.Roles.Find(RoleId);
    if (role != null)
    {
        User user = db.Users.Find(UserId);
        if (user != null)
        {
            if (role.Users == null)
            {
                role.Users = new List<User>();
            }
            role.Users.Add(user);
            db.Entry(role).State = EntityState.Modified;
            if (user.Roles == null)
            {
                user.Roles = new List<Role>();
            }
            user.Roles.Add(role);
            db.Entry(user).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
        else
        {
            return Results.BadRequest("UserId mismatch");
        }
    }
    else
    {
        return Results.BadRequest("UserId mismatch");
    }
    
    return Results.NoContent();
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
// app.MapPost("/users", async (User user, ApplicationDbContext dbContext) => 
// {
//     dbContext.Users.Add(user);
//     await dbContext.SaveChangesAsync(); 
//     return Results.Created($"/users/{user.Id}", user);
// });

app.MapPost("start_fetch", async (StartFetcher fetcher, ApplicationDbContext dbContext) =>
{
    dbContext.Currencies.ExecuteDelete();
    dbContext.Categories.ExecuteDelete();
    dbContext.SaveChanges(); 

    int numCoins = fetcher.NumCoins;
    int numCategories = fetcher.NumCategories; 

    try
    {
        var result = ApiCaller.getCategories().Result;
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
        }

        var listCategories = dbContext.Categories.ToList();

        foreach (var category in listCategories)
        {
            string cmcId = category.CMCCategoryId;
            string response = await ApiCaller.getCategoryWithCoins(cmcId);
            Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(response);

            var coins = new List<Currency>();

            foreach (var coin in myDeserializedClass.data.coins)
            {
                var random = new Random();
                int id = random.Next();

                var currency = new Currency();

                currency.CurrencyId = id;
                currency.CategoryId = category.CategoryId;
                currency.CurrencyName = coin.name;
                currency.TotalSupply = coin.total_supply;
                currency.CMCId = Convert.ToString(coin.id);


                if (coin.quote != null)
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

                Thread.Sleep(1000);
            }

            category.Coins = coins;
            dbContext.Update(category);
            dbContext.SaveChanges();
        }
    }
    catch (Exception)
    {
    }
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
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())  
{  
//     var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();  
//     var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
//     //var emailStore = scope.ServiceProvider.GetRequiredService<IUserEmailStore<User>>();
        ApplicationDbContext dbContext = new ApplicationDbContext();
    


//     string roleName = "Admin";
//     var roleStore = new RoleStore<Role>(dbContext);

//     if (!dbContext.Roles.Any(r => r.Name == roleName))
//     {
//         await roleStore.CreateAsync(new Role(roleName));
//     }
//     User admin = new User();
//     if (!dbContext.Users.Any(u => u.UserName == admin.UserName))
//     {
        
//         var password = new PasswordHasher<User>();
//         var hashed = password.HashPassword(admin,"youshallbeasgods");
//         admin.PasswordHash = hashed;

//         var userStore = new UserStore<User>(dbContext);
//         var result = userStore.CreateAsync(admin);
//     }
//     await dbContext.SaveChangesAsync();
//     await userManager.AddToRoleAsync(admin, "Admin");

//     // if (!await roleManager.RoleExistsAsync(roleName))  
//     // {
//     //     Role role = new Role(roleName);
//     //     await roleManager.CreateAsync(role);
//     //     //dbContext.Roles.Add(role);  
//     //     //await dbContext.SaveChangesAsync();
//     // }  
//     // if (await userManager.FindByNameAsync("admin") == null)  
//     // {
//     //     User admin = new User();
//     //     await userManager.SetUserNameAsync(admin, "admin");
//     //     await userManager.SetEmailAsync(admin, "grantrynders@outlook.com");
//     //     admin.EmailConfirmed = true;
//     //     //await emailStore.SetEmailConfirmedAsync(admin, true, CancellationToken.None);
//     //     await userManager.CreateAsync(admin, "password");
//     //     await userManager.AddToRoleAsync(admin, "Admin");
//     // }  
    
}  

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();