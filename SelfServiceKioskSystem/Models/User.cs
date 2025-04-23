using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelfServiceKioskSystem.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string AccountStatus { get; set; }
        public string Password { get; set; }

        public int RoleID { get; set; }
        public Role Role { get; set; }

        public Wallet Wallet { get; set; }
        public Cart Carts { get; set; }
        public List<TransactionDetail> Transaction { get; set; }
    }


}
