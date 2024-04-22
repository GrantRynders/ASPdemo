using System.Net;
using ASPdemo.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
namespace ASPdemo.Database;
public class ApplicationDbContext : IdentityDbContext<User, Role, string, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
{
    public DbSet<PortfolioToken> PortfolioTokens { get; set; }
    public DbSet<Conversion> Conversions {  get; set; }
    public DbSet<Portfolio> Portfolios { get; set; }
    public override DbSet<User> Users {get; set;}
    public override DbSet<Role> Roles { get; set; } //Roles table containing Admin
    public DbSet<Currency> Currencies {get; set;}
    public DbSet<Category> Categories {get; set;}
    public DbSet<CurrenciesPortfolios> CurrenciesPortfolios {get; set;} // Join table
    public DbSet<IdentityUserClaim<string>> IdentityUserClaim { get; set; }  //this was necessary to get the identity system set up, but we really aren't going to use it beyond that
    //claims are basically really overcomplicated ways of saying "this role/user has permissions to do XYZ" which we could just do with simple booleans or property reads so they really aren't useful
    public DbSet<IdentityRoleClaim<string>> IdentityRoleClaim { get; set; }  
    //public DbSet<UsersRoles> UsersRoles { get; set; }
    public String DbPath {get; set;}
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) // IN MEMORY DB CONSTRUCTOR Note: currently not in use
    {
    }

    public ApplicationDbContext() // MIGRATION DATABASE CONSTRUCTOR
    {
        //Database.EnsureCreated();
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath =System.IO.Path.Join(path, "crypto.db");

        Database.EnsureCreated(); 
    }
    

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder) //data seeding
    {
        //base.OnModelCreating(modelBuilder);
        Console.WriteLine("------------------------------------------OnModelCreating Initial");
        modelBuilder.Entity<IdentityUserClaim<string>>().HasKey(p => new { p.Id }); 
        modelBuilder.Entity<IdentityRoleClaim<string>>().HasKey(p => new { p.Id });
        modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(p => new { p.UserId });
        modelBuilder.Entity<IdentityUserToken<string>>().HasKey(p => new { p.UserId });
        modelBuilder.Entity<IdentityUserRole<string>>().HasKey(p => new { p.UserId, p.RoleId });
        // modelBuilder.Entity<User>(b =>
        // {
        //     // Each User can have many entries in the UserRole join table
        //     b.HasMany(e => e.usersRoles)
        //         .WithOne(e => e.user)
        //         .HasForeignKey(ur => ur.UserId)
        //         .IsRequired();
        // });

        // modelBuilder.Entity<Role>(b =>
        // {
        //     // Each Role can have many entries in the UserRole join table
        //     b.HasMany(e => e.usersRoles)
        //         .WithOne(e => e.role)
        //         .HasForeignKey(ur => ur.RoleId)
        //         .IsRequired();
        // });

        modelBuilder.Entity<Portfolio>() //keep: this works
        .HasOne(e => e.user)
        .WithOne(e => e.portfolio)
        .HasForeignKey<Portfolio>(e => e.UserId)
        .IsRequired();







        //seeding user and role table

        var hasher = new PasswordHasher<User>();
        modelBuilder.Entity<Role>().HasData(
            new Role() 
            {
                Id = "2c5e174e-3b0e-446f-86af-483d56fd7210",
                Name = "admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            });
        modelBuilder.Entity<User>().HasData(
            new User()
            {
                Id = "8e445865-a24d-4543-a6c6-9443d048cdb9", // primary key
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                EmailConfirmed = true,
                Email = "grantrynders@outlook.com",
                NormalizedEmail = "GRANTRYNDERS@OUTLOOK.COM",
                PasswordHash = hasher.HashPassword(null, "youshallbeasgods"),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            }
        );
        //Seeding the relation between our user and role to AspNetUserRoles table
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>()
            {
                RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210", 
                UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
            }
        );
    }





    //#############################################################################
    //COMMANDS:
    //dotnet ef migrations add ""
    //dotnet ef database update
}