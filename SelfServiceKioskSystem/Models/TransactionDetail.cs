using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace SelfServiceKioskSystem.Models
{
    public class TransactionDetail
    {
        [Key]
        public int TransactionID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        [ForeignKey("Wallet")]
        public int WalletID { get; set; }

        [ForeignKey("Cart")]
        public int CartID { get; set; }

        public DateTime TransactionDate { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public User User { get; set; }

        public Wallet Wallet { get; set; }

        public Cart Cart { get; set; }


    }
}
