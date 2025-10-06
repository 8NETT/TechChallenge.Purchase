using TechChallenge.Purchases.Core.Entity;
using TechChallenge.Purchases.Core.Exceptions;
using TechChallenge.Purchases.Core.Extensions;
using TechChallenge.Purchases.Core.Validators;
using FluentValidation.Results;
using TechChallenge.Purchases.Core.Enum;

namespace TechChallenge.Purchases.Core.Builders
{
    public sealed class CompraBuilder
    {
        private Compra _compra = new Compra();

        public CompraBuilder Id(int id) => this.Tee(b => b._compra.Id = id);
        public CompraBuilder CompradorId(int id) => this.Tee(b => b._compra.CompradorId = id);
        public CompraBuilder JogoId(Guid id) => this.Tee(b => b._compra.JogoId = id);
        public CompraBuilder Valor(decimal valor) => this.Tee(b => b._compra.Valor = valor);
        public CompraBuilder Desconto(int desconto) => this.Tee(b => b._compra.Desconto = desconto);
        public CompraBuilder Total(decimal total) => this.Tee(b => b._compra.Total = total);
        public CompraBuilder CreatedAt(DateTime data) => this.Tee(b => b._compra.CreatedAt = data);
        public CompraBuilder PaymentMethodType(EPaymentMethodType type) => this.Tee(b => b._compra.PaymentMethodType = type);
        
        

        public ValidationResult Validate() =>
            new CompraValidator().Validate(_compra);

        public Compra Build()
        {
            if (!Validate().IsValid)
                throw new EstadoInvalidoException("Não é possível criar uma compra em um estado inválido.");

            return _compra;
        }
    }
}