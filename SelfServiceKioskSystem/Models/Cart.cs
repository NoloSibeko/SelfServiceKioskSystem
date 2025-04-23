using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;
using Microsoft.EntityFrameworkCore;

namespace SelfServiceKioskSystem.Models
{
    public class Cart
    {
        [Key]
        public int CartID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        [ForeignKey("Wallet")]
        public int WalletID { get; set; }

        public int Quantity { get; set; }

        public decimal TotalAmount { get; set; }

        public User User { get; set; }

        public TransactionDetail Transaction { get; set; }
        public ICollection<Product> Products { get; set; }

    }
}
