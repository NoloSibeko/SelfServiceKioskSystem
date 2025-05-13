using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SelfServiceKioskSystem.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [ForeignKey("Category")]
        public int CategoryID { get; set; }

        public decimal Price { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public string Description { get; set; }

        public string ImageURL { get; set; }

        public Boolean isAvailable { get; set; } = true;

        public Category Category { get; set; }

        public ICollection<Cart> Carts { get; set; }
    }
}
