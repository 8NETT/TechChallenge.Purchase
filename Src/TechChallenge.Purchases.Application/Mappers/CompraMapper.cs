using TechChallenge.Purchases.Application.DTOs;
using TechChallenge.Purchases.Core.Entity;

namespace TechChallenge.Purchases.Application.Mappers
{
    public static class CompraMapper
    {
        public static CompraDTO ToDTO(this Compra entidade) => new CompraDTO
        {
            Id = entidade.Id,
            UsuarioId = entidade.CompradorId,
            JogoId = entidade.JogoId,
            Valor = entidade.Valor,
            Desconto = entidade.Desconto,
            Total = entidade.Total ?? 0,
            PaymentMethodType = entidade.PaymentMethodType
        };
        
        public static Compra ToEntity(this CompraDTO dto) => Compra.New()
            .Id(dto.Id)
            .CompradorId(dto.UsuarioId)
            .CreatedAt(DateTime.Now)
            .PaymentMethodType(dto.PaymentMethodType)
            .JogoId(dto.JogoId)
            .Valor(dto.Valor)
            .Desconto(dto.Desconto)
            .Total(dto.Total)
            .Build();
    }
}