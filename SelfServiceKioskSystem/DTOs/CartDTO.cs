namespace SelfServiceKioskSystem.DTOs
{
    public class CartDTO
    {
        public int CartID { get; set; }
        public int UserID { get; set; }
        public decimal TotalAmount { get; set; }
        public List<CartItemDTO> Items { get; set; }
    }
}
