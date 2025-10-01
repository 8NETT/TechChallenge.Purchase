using TechChallenge.Purchases.Core.Entity;
using FluentValidation;

namespace TechChallenge.Purchases.Core.Validators
{
    public class CompraValidator : AbstractValidator<Compra>
    {
        public CompraValidator()
        {
            RuleFor(c => c.CreatedAt)
                .NotEmpty();

            RuleFor(c => c.JogoId)
                .NotEmpty().WithMessage("O ID do jogo deve ser preenchido.");

            RuleFor(c => c.CompradorId)
                .NotEmpty().WithMessage("O ID comprador deve ser preenchido.");

            RuleFor(c => c.Valor)
                .GreaterThanOrEqualTo(0M).WithMessage("O valor não pode ser negativo.");

            RuleFor(c => c.Desconto)
                .GreaterThanOrEqualTo(0).WithMessage("O desconto não pode ser negativo.")
                .LessThanOrEqualTo(100).WithMessage("O desconto não pode ser maior que 100.");

            RuleFor(c => c.Total)
                .GreaterThanOrEqualTo(0M).WithMessage("O total não pode ser negativo.");
        }
    }
}