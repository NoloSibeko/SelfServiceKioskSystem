using System.ComponentModel.DataAnnotations;

namespace SelfServiceKioskSystem.DTOs
{
    public class UpdateUserDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [RegularExpression(@"^[^@\s]+@singular\.co\.za$", ErrorMessage = "Email must be @singular.co.za")]
        public string Email { get; set; }

        [Required]
        public string ContactNumber { get; set; }

        [Required]
        [MinLength(8)]
        [RegularExpression(@"^(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Password not strong or long enough")]
        public string Password { get; set; }
    }
}
