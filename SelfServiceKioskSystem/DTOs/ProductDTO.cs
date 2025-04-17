namespace SelfServiceKioskSystem.Models.DTOs
{
    public class ProductDTO
    {
        public string Name { get; set; }
        public string ImageURL { get; set; } 
        public string CategoryName { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
    }
}
