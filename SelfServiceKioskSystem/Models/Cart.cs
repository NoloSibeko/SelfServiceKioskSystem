using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;
using Microsoft.EntityFrameworkCore;

namespace SelfServiceKioskSystem.Models
{
    public class Cart
    {
        public int CartID { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
        public decimal TotalAmount { get; set; }
        public TransactionDetail Transaction { get; set; }
        public List<CartItem> CartItems { get; set; }

    }
}
