using System;
using System.Net;
using System.Web;
using ASPdemo;
using ASPdemo.Database;
using ASPdemo.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

//CREATE SQLITE DB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite("Data Source=Crypto.db");
});




// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();





//############## CRUD OPS #######################

// REQUEST 1: get all users
app.MapGet("/users/{maxId}/{pageId}", async (ApplicationDbContext dbContext) => 
{
    return await dbContext.Users.ToListAsync(); 
});
app.MapGet("/roles/{maxId}/{pageId}", async (ApplicationDbContext dbContext) => 
{
    return await dbContext.Roles.ToListAsync(); 
});
// get listings
app.MapGet("/listings/{maxId}/{pageId}", async (int maxId, int pageId, ApplicationDbContext db) =>
{
    var listings = db.Currencies.Where(x => x.CurrencyId <= maxId).Where(x => x.CurrencyId >= pageId)
    .ToList();

    return listings;
});

//get quotes
app.MapGet("/quotes/{maxId}/{pageId}", async (int maxId, int pageId, ApplicationDbContext dbContext) =>
{
});

//get categories
app.MapGet("/categories/{maxId}/{pageId}", async (int maxId, int pageId, ApplicationDbContext db) =>
{
    var categories = db.Categories.Where(x => x.CategoryId <= maxId)
                        .Where(x => x.CategoryId >= pageId) .ToList();
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

// ApplicationDbContext dbContext = new ApplicationDbContext();
// List<User> adminPromotees = new List<User>();
//                     User? user = dbContext.Users.Find("ccda0ef3-b1ca-4084-9294-bb4a35ea1c75");
//                     Console.WriteLine(user.UserName);
//                     adminPromotees.Add(user);
//                     dbContext.Roles.Add(new Admin { Users = new List<User>(adminPromotees)});