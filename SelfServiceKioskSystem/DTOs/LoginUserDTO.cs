using System.ComponentModel.DataAnnotations;

namespace SelfServiceKioskSystem.Models.DTOs
{
    public class LoginUserDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
