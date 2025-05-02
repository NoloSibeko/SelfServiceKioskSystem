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

            // Define relationships

            modelBuilder.Entity<User>()
                .HasOne(u => u.Wallet)
                .WithOne(w => w.User)
                .HasForeignKey<Wallet>(w => w.UserID);

            // Commented out as it seems to be an incomplete relationship setup
            // modelBuilder.Entity<User>()
            //     .HasOne(u => u.Carts)
            //     .WithOne(c => c.User)
            //     .HasForeignKey<Cart>(c => c.UserID);

            // Prevent cascading delete on User for TransactionDetail
            modelBuilder.Entity<TransactionDetail>()
                .HasOne(td => td.User)
                .WithMany() // Adjust as needed for your model
                .HasForeignKey(td => td.UserID)
                .OnDelete(DeleteBehavior.NoAction); // No cascading delete

            modelBuilder.Entity<User>()
                .HasMany(u => u.Transaction)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserID);

            modelBuilder.Entity<Wallet>()
                .HasMany(w => w.TransactionDetails)
                .WithOne(t => t.Wallet)
                .HasForeignKey(t => t.WalletID);

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryID);

            // Many-to-many relationship between Cart and Product
            modelBuilder.Entity<Cart>()
                .HasMany(c => c.Products)
                .WithMany(p => p.Carts)
                .UsingEntity(j => j.ToTable("CartProducts"));

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Transaction)
                .WithOne(t => t.Cart)
                .HasForeignKey<TransactionDetail>(t => t.CartID);

            // Decimal Precision Configuration
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

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryID = 1, CategoryName = "Hot Beverages" },
                new Category { CategoryID = 2, CategoryName = "Cold Drinks" },
                new Category { CategoryID = 3, CategoryName = "Snacks" },
                new Category { CategoryID = 4, CategoryName = "Hot Meals" },
                new Category { CategoryID = 5, CategoryName = "Desserts" }
            );

            // Add appropriate cascade delete settings for TransactionDetail, Cart, etc.

            // Prevent cascading delete between Cart and TransactionDetails if needed
            modelBuilder.Entity<TransactionDetail>()
                .HasOne(td => td.Cart)
                .WithOne(c => c.Transaction)
                .HasForeignKey<TransactionDetail>(td => td.CartID)
                .OnDelete(DeleteBehavior.Restrict);  // Restrict cascading delete

            // If you want to restrict cascading delete for Cart itself, do so here:
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Transaction)
                .WithOne(t => t.Cart)
                .HasForeignKey<TransactionDetail>(t => t.CartID)
                .OnDelete(DeleteBehavior.Restrict); // Restrict cascading delete
        }
    }
}
