using System.ComponentModel.DataAnnotations;

namespace TechChallenge.Purchases.Application.DTOs
{
    public class CadastrarUsuarioDTO
    {
        [Required(ErrorMessage = "O campo {0} deve ser preenchido.")]
        [Length(5, 100, ErrorMessage = "O campo {0} deve ter entre 5 a 100 caracteres.")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} deve ser preenchido.")]
        [EmailAddress(ErrorMessage = "O campo {0} não é uma email válido.")]
        [StringLength(100, ErrorMessage = "O campo {0} deve ter no máximo 100 caracteres.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} deve ser preenchido.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$", ErrorMessage = "A senha precisa conter caractere especial, letra e número.")]
        [StringLength(60, MinimumLength = 8, ErrorMessage = "A senha deve ter entre 8 e 60 caracteres.")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "O campo {0} deve ser preenchido.")]
        public required bool Profile { get; set; }
    }
}
