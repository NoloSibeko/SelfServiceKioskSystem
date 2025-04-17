using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using SelfServiceKioskSystem.Models.Enums;

namespace SelfServiceKioskSystem.Models
{
    public class Role
    {
        [Key]
        public int RoleID { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserID { get; set; }

        [Required]
        public string UserRole { get; set; }

        public User User { get; set; }

        //[Required]
        //public AdminRole AdminRole { get; set; }
    }
}
