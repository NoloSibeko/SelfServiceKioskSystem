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
            // User-Wallet
            modelBuilder.Entity<User>()
                .HasOne(u => u.Wallet)
                .WithOne(w => w.User)
                .HasForeignKey<Wallet>(w => w.UserID);

            // User-Admin
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithOne(a => a.User)
                .HasForeignKey<Role>(a => a.UserID);

            // User-Cart
            modelBuilder.Entity<User>()
                .HasOne(u => u.Carts)
                .WithOne(c => c.User)
                .HasForeignKey<Cart>(c => c.UserID);

            // User-Transactions
            modelBuilder.Entity<User>()
                .HasMany(u => u.Transaction)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserID);

            // Wallet-Transactions
            modelBuilder.Entity<Wallet>()
                .HasMany(w => w.TransactionDetails)
                .WithOne(t => t.Wallet)
                .HasForeignKey(t => t.WalletID);

            // User-Products
           /* modelBuilder.Entity<User>()
                .HasMany(u => u.Products)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserID);*/

            // Category-Products
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryID);

            // Cart-Products many-to-many
            modelBuilder.Entity<Cart>()
                .HasMany(c => c.Products)
                .WithMany(p => p.Carts)
                .UsingEntity(j => j.ToTable("CartProducts"));

            // Cart-Transaction
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

            modelBuilder.Entity<Role>().HasData(
                new Role { RoleID = 1, UserRole = "User" },
                new Role { RoleID = 2, UserRole = "Superuser" }
);

            base.OnModelCreating(modelBuilder);
        }


    }
}
