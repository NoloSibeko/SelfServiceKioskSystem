namespace SelfServiceKioskSystem.DTOs
{
    public class AddToCartDTO
    {
        public int UserID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }// List of items in the cart
    }
}
