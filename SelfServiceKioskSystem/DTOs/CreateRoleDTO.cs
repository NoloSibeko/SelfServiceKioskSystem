using System.ComponentModel.DataAnnotations;

namespace SelfServiceKioskSystem.DTOs
{
    public class CreateRoleDTO
    {
        [Required]
        public int UserID { get; set; }

        [Required]
        public string UserRole { get; set; }
    }
}
