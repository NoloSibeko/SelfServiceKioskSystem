namespace SelfServiceKioskSystem.DTOs
{
    public class UpdateProductDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int CategoryID { get; set; }
        public IFormFile? Image { get; set; }
    }
}
