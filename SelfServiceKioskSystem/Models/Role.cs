using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using SelfServiceKioskSystem.Models.Enums;

namespace SelfServiceKioskSystem.Models
{
    public class Role
    {
        [Key]
        public int RoleID { get; set; }
        public string RoleName { get; set; }

        // Navigation property
        public ICollection<User> Users { get; set; }
    }

}
