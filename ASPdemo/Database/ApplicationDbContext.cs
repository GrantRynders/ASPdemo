using System.Net;
using ASPdemo.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
namespace ASPdemo.Database;
public class ApplicationDbContext : IdentityDbContext<User, Role, string, IdentityUserClaim<string>, UsersRoles, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
{
    public DbSet<Portfolio> Portfolios { get; set; }
    public override DbSet<User> Users {get; set;}
    public override DbSet<Role> Roles { get; set; } //Roles table containing Admin
    public DbSet<Currency> Currencies {get; set;}
    public DbSet<Category> Categories {get; set;}
    public DbSet<CurrenciesPortfolios> CurrenciesPortfolios {get; set;} // Join table
    public DbSet<IdentityUserClaim<string>> IdentityUserClaim { get; set; }  //this was necessary to get the identity system set up, but we really aren't going to use it beyond that
    //claims are basically really overcomplicated ways of saying "this role/user has permissions to do XYZ" which we could just do with simple booleans or property reads so they really aren't useful
    public DbSet<IdentityRoleClaim<string>> IdentityRoleClaim { get; set; }  
    public DbSet<UsersRoles> UsersRoles { get; set; }
    public String DbPath {get; set;}
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) // IN MEMORY DB CONSTRUCTOR Note: currently not in use
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
        //base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<IdentityUserClaim<string>>().HasKey(p => new { p.Id }); 
        modelBuilder.Entity<IdentityRoleClaim<string>>().HasKey(p => new { p.Id });
        modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(p => new { p.UserId });
        modelBuilder.Entity<IdentityUserToken<string>>().HasKey(p => new { p.UserId });
        //modelBuilder.Entity<IdentityUserRole<string>>();
        //modelBuilder.Entity<User>().HasKey(p => new { p.Id });
        //modelBuilder.Entity<Role>().HasKey(p => new { p.Id });

        modelBuilder.Entity<Portfolio>() //keep: this works
        .HasOne(e => e.user)
        .WithOne(e => e.portfolio)
        .HasForeignKey<Portfolio>(e => e.UserId)
        .IsRequired();
    }





    //#############################################################################
    //COMMANDS:
    //dotnet ef migrations add ""
    //dotnet ef database update
}