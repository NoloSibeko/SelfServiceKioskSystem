using System.ComponentModel.DataAnnotations;

namespace SelfServiceKioskSystem.DTOs
{
    public class CreateProductDTO
    {
        [Required]
        [StringLength(50, ErrorMessage = "Product name is too long")]
        public string Name { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Description is too long")]
        public string Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0." )]
        public decimal Price { get; set; }

       // public byte[] ImageURL { get; set; }

        public bool IsAvailable { get; set; }

        public int Quantity    { get; set; }

        [Required]
        public int CategoryID { get; set; }

        public IFormFile Image { get; set; }
    }
}
