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
            Total = entidade.Total
        };
    }
}