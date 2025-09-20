using System.ComponentModel.DataAnnotations;

namespace TechChallenge.Purchases.Application.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "O campo {0} deve ser preenchido.")]
        [EmailAddress(ErrorMessage = "O campo {0} não é uma email válido.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} deve ser preenchido.")]
        public required string Password { get; set; }
    }
}
