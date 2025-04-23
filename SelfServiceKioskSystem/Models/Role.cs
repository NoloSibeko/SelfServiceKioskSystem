using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using SelfServiceKioskSystem.Models.Enums;

namespace SelfServiceKioskSystem.Models
{
    public class Role
    {
        public int RoleID { get; set; }
        public string UserRole { get; set; }
        public ICollection<User> Users { get; set; }
    }


}
