using System.ComponentModel.DataAnnotations;

namespace SelfServiceKioskSystem.DTOs
{
    public class AddWalletFundsDTO
    {
        public int UserId { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be a positive number")]
        public decimal Amount { get; set; }
    }
}
