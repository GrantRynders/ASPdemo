using System.Net;
using ASPdemo.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
namespace ASPdemo.Database;
public class ApplicationDbContext : DbContext
{
    
    public DbSet<User> Users {get; set;}
    public DbSet<Currency> Currencies {get; set;}
    public DbSet<Category> Categories {get; set;}
    public DbSet<CurrenciesCategories> CurrenciesCategories {get; set;} // Join table
    public DbSet<Airdrop> Airdrops {get; set;}
    //public DbSet<Admin> Admins {get; set;} Admin really should just be in the users table right?
    public String DbPath {get; set;}
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {
        //currently dbcontext is configured like an inmemory db, included the ef db methods too


        //var folder = Environment.SpecialFolder.LocalApplicationData;
        //var path = Environment.GetFolderPath(folder);
        //DbPath =System.IO.Path.Join(path, "crypto.db");
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder) //seeding user data table
    {
        modelBuilder.Entity<User>().HasData(
            new Admin {UserId = 1, PermissionsLevel = 1, FirstName = "Grant", LastName = "Rynders", Email = "ryndergb@mail.uc.edu", followedCurrencies = new List<Currency>()}
            );
    }





    //#############################################################################
    //COMMANDS:
    //dotnet ef migrations add ""
}