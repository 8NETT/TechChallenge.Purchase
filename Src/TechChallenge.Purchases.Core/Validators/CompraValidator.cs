using TechChallenge.Purchases.Core.Entity;
using FluentValidation;

namespace TechChallenge.Purchases.Core.Validators
{
    internal class CompraValidator : AbstractValidator<Compra>
    {
        public CompraValidator()
        {
            RuleFor(c => c.DataCriacao)
                .NotEmpty();

            RuleFor(c => c.JogoId)
                .NotEmpty().WithMessage("O ID do jogo deve ser preenchido.");

            RuleFor(c => c.CompradorId)
                .NotEmpty().WithMessage("O ID comprador deve ser preenchido.");

            RuleFor(c => c.Valor)
                .NotEmpty().WithMessage("O valor deve ser preenchido.")
                .GreaterThanOrEqualTo(0M).WithMessage("O valor não pode ser negativo.");

            RuleFor(c => c.Desconto)
                .GreaterThanOrEqualTo(0).WithMessage("O desconto não pode ser negativo.")
                .LessThanOrEqualTo(100).WithMessage("O desconto não pode ser maior que 100.");

            RuleFor(c => c.Total)
                .NotEmpty().WithMessage("O total deve ser preenchido.")
                .GreaterThanOrEqualTo(0M).WithMessage("O total não pode ser negativo.");
        }
    }
}