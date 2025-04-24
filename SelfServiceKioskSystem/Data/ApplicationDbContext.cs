using Microsoft.EntityFrameworkCore;
using SelfServiceKioskSystem.Models;

namespace SelfServiceKioskSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<TransactionDetail> TransactionDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

      
            modelBuilder.Entity<User>()
                .HasOne(u => u.Wallet)
                .WithOne(w => w.User)
                .HasForeignKey<Wallet>(w => w.UserID);

            
           /* modelBuilder.Entity<User>()
                .HasOne(u => u.Carts)
                .WithOne(c => c.User)
                .HasForeignKey<Cart>(c => c.UserID);*/

           /* 
            modelBuilder.Entity<User>()
                .HasMany(u => u.Transaction)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserID);*/

            
            modelBuilder.Entity<Wallet>()
                .HasMany(w => w.TransactionDetails)
                .WithOne(t => t.Wallet)
                .HasForeignKey(t => t.WalletID);

            
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryID);

            
            modelBuilder.Entity<Cart>()
                .HasMany(c => c.Products)
                .WithMany(p => p.Carts)
                .UsingEntity(j => j.ToTable("CartProducts"));

            
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Transaction)
                .WithOne(t => t.Cart)
                .HasForeignKey<TransactionDetail>(t => t.CartID);

            
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Cart>()
                .Property(c => c.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<TransactionDetail>()
                .Property(t => t.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Wallet>()
                .Property(w => w.Balance)
                .HasPrecision(18, 2);

            // Seed Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleID = 1, UserRole = "User" },
                new Role { RoleID = 2, UserRole = "Superuser" }
            );

           /* // Seed Wallet for the superuser
            modelBuilder.Entity<Wallet>().HasData(
                new Wallet
                {
                    WalletID = 1,
                    UserID = 1, 
                    Balance = 0
                }
            );

            // Seed SuperUser
            modelBuilder.Entity<User>().HasData(

                new User {
                    UserID = 1, 
                    Name = "Bonolo",
                    Surname = "Sibeko",
                    Email = "bonolo@singular.co.za",
                    ContactNumber = "0620912838",
                    Password = BCrypt.Net.BCrypt.HashPassword("BonoloSibeko123#"),
                    AccountStatus = "Active",
                    RoleID = 2 
                }
            );*/

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryID = 1, CategoryName = "Hot Beverages" },
                new Category { CategoryID = 2, CategoryName = "Cold Drinks" },
                new Category { CategoryID = 3, CategoryName = "Snacks" },
                new Category { CategoryID = 4, CategoryName = "Hot Meals" },
                new Category { CategoryID = 5, CategoryName = "Desserts" }
            );

           
        }
    }
}
