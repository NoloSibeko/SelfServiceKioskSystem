using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelfServiceKioskSystem.Models
{
    public class Wallet
    {
        public int WalletID { get; set; }
        public decimal Balance { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }

        public List<TransactionDetail> TransactionDetails { get; set; }
    }

}
