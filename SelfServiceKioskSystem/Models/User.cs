using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelfServiceKioskSystem.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        public string UserRole { get; set; } = "User"; // Default role

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Password { get; set; }

        public string AccountStatus { get; set; } = "Active";

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string ContactNumber { get; set; }

        public Wallet Wallet { get; set; }

        public Role Role { get; set; }

        public Cart Carts { get; set; }

        public ICollection<TransactionDetail> Transaction { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
