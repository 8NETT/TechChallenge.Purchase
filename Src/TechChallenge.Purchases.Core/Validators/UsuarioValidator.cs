using TechChallenge.Purchases.Core.Entity;
using FluentValidation;
using System.Net.Mail;

namespace TechChallenge.Purchases.Core.Validators
{
    internal class UsuarioValidator : AbstractValidator<Usuario>
    {
        public UsuarioValidator()
        {
            RuleFor(u => u.DataCriacao)
                .NotEmpty();

            RuleFor(u => u.Nome)
                .NotEmpty().WithMessage("O nome tem de ser preenchido.")
                .MinimumLength(5).WithMessage("O login tem de ter no mínimo 5 caracteres.");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("O email tem de ser preenchido.")
                .Must(BeAValidEmail).WithMessage("O email tem de estar em um formato válido.");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("A senha tem de ser preenchida.");
        }

        private bool BeAValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
    }
}
