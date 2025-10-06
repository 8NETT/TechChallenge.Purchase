using TechChallenge.Purchases.Core.Enum;

namespace TechChallenge.Purchases.Application.DTOs
{
    public class CompraDTO
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public required Guid JogoId { get; set; }
        public required decimal Valor { get; set; }
        public required int Desconto { get; set; }
        public decimal Total { get; set; }
        
        public EPaymentMethodType PaymentMethodType { get; set; }

        public PaymentMethodDTO? PaymentMethod { get; set; } = new();
    }
}
