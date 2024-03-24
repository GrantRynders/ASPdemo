using System;
using System.Net;
using System.Web;
using ASPdemo;
using ASPdemo.Database;
using ASPdemo.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

//CREATE MIGRATION DB
//using var db = new ApplicationDbContext();

//CREATE IN MEMORY DB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite("Data Source=Crypto.db");
});

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();
//app.Urls.Add("http://localhost:5220");

//############## CRUD OPS #######################

// REQUEST 1: get all users
app.MapGet("/users", async (ApplicationDbContext dbContext) => 
{
    return await dbContext.Users.ToListAsync(); 
});

app.MapGet("/listings/{maxId}/{pageId}", async (int maxId, int pageId, ApplicationDbContext db) =>
{
    var listings = db.Currencies.Where(x => x.CurrencyId <= maxId).Where(x => x.CurrencyId >= pageId)
    .ToList();

    return listings;
});

app.MapGet("/airdrops/{maxId}/{pageId}", async (int maxId, int pageId, ApplicationDbContext dbContext) =>
{
});

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
    return Results.Created($"/users/{user.UserId}", user);
});

// REQUEST 6: update a user

app.MapPut("/users/{userId}", async (int userId, User user, ApplicationDbContext dbContext) => 
{
    if (userId != user.UserId)
    {
        return Results.BadRequest("UserId mismatch");
    }
    dbContext.Entry(user).State = EntityState.Modified;
    await dbContext.SaveChangesAsync();
    return Results.NoContent();
});

// REQUEST 7: delete a user

app.MapDelete("/users/{userId}", async (int userId, ApplicationDbContext dbContext) => 
{
    dbContext.Users.Remove(new User { UserId = userId});
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
