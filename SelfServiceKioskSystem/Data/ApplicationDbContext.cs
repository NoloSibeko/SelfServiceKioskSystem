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

            // Relationships

            // User-Wallet (1:1)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Wallet)
                .WithOne(w => w.User)
                .HasForeignKey<Wallet>(w => w.UserID);

            // User-Cart (1:1)
           /* modelBuilder.Entity<User>()
                .HasOne(u => u.Carts)
                .WithOne(c => c.User)
                .HasForeignKey<Cart>(c => c.UserID);*/

           /* // User-Transactions (1:N)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Transaction)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserID);*/

            // Wallet-Transactions (1:N)
            modelBuilder.Entity<Wallet>()
                .HasMany(w => w.TransactionDetails)
                .WithOne(t => t.Wallet)
                .HasForeignKey(t => t.WalletID);

            // Category-Product (1:N)
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryID);

            // Cart-Products (M:N)
            modelBuilder.Entity<Cart>()
                .HasMany(c => c.Products)
                .WithMany(p => p.Carts)
                .UsingEntity(j => j.ToTable("CartProducts"));

            // Cart-Transaction (1:1)
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Transaction)
                .WithOne(t => t.Cart)
                .HasForeignKey<TransactionDetail>(t => t.CartID);

            // Precision settings
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
                new Role { RoleID = 1, RoleName = "User" },
                new Role { RoleID = 2, RoleName = "Superuser" }
            );

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
