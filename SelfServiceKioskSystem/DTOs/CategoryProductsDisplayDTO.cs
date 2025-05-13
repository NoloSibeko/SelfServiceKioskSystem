namespace SelfServiceKioskSystem.DTOs;
using SelfServiceKioskSystem.Models.DTOs;

    public class CategoryProductsDisplayDTO
    {
    public int CategoryID { get; set; }
    public string CategoryName { get; set; }
    public List<ProductDTO> Products { get; set; }




}


