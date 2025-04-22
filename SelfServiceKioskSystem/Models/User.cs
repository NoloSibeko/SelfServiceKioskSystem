using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelfServiceKioskSystem.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Email { get; set; }

        public string ContactNumber { get; set; }

        public string Password { get; set; }

        public string AccountStatus { get; set; } = "Active";

        public int RoleID { get; set; } = 1; // default to User

        [ForeignKey("RoleID")]
        public Role Role { get; set; }

        // Navigation properties
        public Wallet Wallet { get; set; }
        public Cart Cart { get; set; }
        public List<TransactionDetail> Transactions { get; set; }
    }

}
