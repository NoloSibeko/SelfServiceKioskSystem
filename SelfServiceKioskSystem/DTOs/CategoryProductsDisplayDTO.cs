namespace SelfServiceKioskSystem.DTOs;
using SelfServiceKioskSystem.Models.DTOs;

    public class CategoryProductsDisplayDTO
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public List<ProductDTO> Products { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
       // public string CategoryName { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
       // public int ProductID { get; set; }



    }


