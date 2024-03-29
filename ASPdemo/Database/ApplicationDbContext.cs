using System.Net;
using ASPdemo.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
namespace ASPdemo.Database;
public class ApplicationDbContext : DbContext
{
    public DbSet<Portfolio> Portfolios { get; set; }
    public DbSet<User> Users {get; set;}
    public DbSet<Currency> Currencies {get; set;}
    public DbSet<Category> Categories {get; set;}
    public DbSet<CurrenciesCategories> CurrenciesCategories {get; set;} // Join table
    public DbSet<CurrenciesPortfolios> CurrenciesPortfolios {get; set;} // Join table
    public DbSet<IdentityUserClaim<string>> IdentityUserClaim { get; set; }  //stupid identity crap table
    public DbSet<IdentityRoleClaim<string>> IdentityRoleClaim { get; set; }  //stupid identity crap table
    public DbSet<IdentityRole> Roles { get; set; } //Roles table containing Admin
    public DbSet<UsersRoles> UsersRoles { get; set; }
    public String DbPath {get; set;}
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) // IN MEMORY DB CONSTRUCTOR
    {
    }

    public ApplicationDbContext() // MIGRATION DATABASE CONSTRUCTOR
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath =System.IO.Path.Join(path, "crypto.db");
    }
    

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }
    }
    protected override async void OnModelCreating(ModelBuilder modelBuilder) //data seeding
    {
        modelBuilder.Entity<IdentityUserClaim<string>>().HasKey(p => new { p.Id }); 
        modelBuilder.Entity<IdentityRoleClaim<string>>().HasKey(p => new { p.Id });
        modelBuilder.Entity<IdentityRole>().HasKey(p => new { p.Id });
        modelBuilder.Entity<Portfolio>()
        .HasOne(e => e.user)
        .WithOne(e => e.portfolio)
        .HasForeignKey<Portfolio>(e => e.UserId)
        .IsRequired();

        modelBuilder.Entity<UsersRoles>().HasKey(p => new { p.UsersRolesId });
    }





    //#############################################################################
    //COMMANDS:
    //dotnet ef migrations add ""
    //dotnet ef database update
}