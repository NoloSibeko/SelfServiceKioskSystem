using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelfServiceKioskSystem.Models
{
    public class Wallet
    {
        [Key]
        public int WalletID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        public decimal Balance { get; set; }

        public User User { get; set; }

        public ICollection<TransactionDetail> TransactionDetails { get; set; }
    }
}
