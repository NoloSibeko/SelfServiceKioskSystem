using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelfServiceKioskSystem.Data;


namespace SelfServiceKioskSystem.Models.DTOs
{

    
    public class ProductDTO
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; } 
        public string CategoryName { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
  


    }


}
