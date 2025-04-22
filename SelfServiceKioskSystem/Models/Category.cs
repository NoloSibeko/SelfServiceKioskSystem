using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SelfServiceKioskSystem.Models
{
    public class Category
    {
        public int CategoryID { get; set; }

        [Required]
        [MaxLength(50)]
        public string CategoryName { get; set; }

        // Navigation property
        public ICollection<Product> Products { get; set; }
    }
}