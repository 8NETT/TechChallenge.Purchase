using TechChallenge.Purchases.Core.Builders;
using TechChallenge.Purchases.Core.Enum;

namespace TechChallenge.Purchases.Core.Entity
{
    public class Compra : EntityBase
    {
        public int CompradorId { get; set; }
        public int JogoId { get; set; }
        public decimal Valor { get; set; }
        public int Desconto { get; set; }
        public decimal Total { get; set; }
        public EPaymentMethodType PaymentMethodType { get; set; }
        public bool Estornada { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        
        public void MarcarEstornada() => Estornada = true;
        public Compra() { }
        public static CompraBuilder New() => new CompraBuilder();
    }
}