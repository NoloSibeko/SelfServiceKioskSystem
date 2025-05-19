namespace SelfServiceKioskSystem.DTOs
{
    public class TransactionDetailDTO
    {
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public decimal ResultingBalance { get; set; }
    }
}
