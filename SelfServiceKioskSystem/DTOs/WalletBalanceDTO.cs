namespace SelfServiceKioskSystem.DTOs
{
    public class WalletBalanceDTO
    {
        public decimal Balance { get; set; }
        public List<TransactionDetailDTO> Transactions { get; set; }
    }
}
