using TechChallenge.Purchases.Application.DTOs;
using TechChallenge.Purchases.Application.Mappers;
using TechChallenge.Purchases.Core.Entity;
using TechChallenge.Purchases.Core.Enum;
using Xunit;

namespace TechChallenge.Purchases.Tests.Application.Mappers
{
    public class CompraMapperTests
    {
        [Fact]
        public void DeveMapearParaDTO_QuandoEntidadeValida()
        {
            // Arrange
            var compra = new Compra
            {
                Id = 1,
                CompradorId = 123,
                JogoId = 456,
                Valor = 100.50m,
                Desconto = 10,
                Total = 90.45m,
                PaymentMethodType = EPaymentMethodType.Credit
            };

            // Act
            var dto = compra.ToDTO();

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(compra.Id, dto.Id);
            Assert.Equal(compra.CompradorId, dto.UsuarioId);
            Assert.Equal(compra.JogoId, dto.JogoId);
            Assert.Equal(compra.Valor, dto.Valor);
            Assert.Equal(compra.Desconto, dto.Desconto);
            Assert.Equal(compra.Total, dto.Total);
            Assert.Equal(compra.PaymentMethodType, dto.PaymentMethodType);
        }

        [Fact]
        public void DeveMapearTotalNuloParaZero_QuandoMapeandoParaDTO()
        {
            // Arrange
            var compra = new Compra
            {
                Total = null
            };

            // Act
            var dto = compra.ToDTO();

            // Assert
            Assert.Equal(0, dto.Total);
        }

        [Fact]
        public void DeveMapearParaEntidade_QuandoDTOValido()
        {
            // Arrange
            var dto = new CompraDTO
            {
                Id = 1,
                UsuarioId = 123,
                JogoId = 456,
                Valor = 100.50m,
                Desconto = 10,
                Total = 90.45m,
                PaymentMethodType = EPaymentMethodType.Debit
            };

            // Act
            var compra = dto.ToEntity();

            // Assert
            Assert.NotNull(compra);
            Assert.Equal(dto.Id, compra.Id);
            Assert.Equal(dto.UsuarioId, compra.CompradorId);
            Assert.Equal(dto.JogoId, compra.JogoId);
            Assert.Equal(dto.Valor, compra.Valor);
            Assert.Equal(dto.Desconto, compra.Desconto);
            Assert.Equal(dto.Total, compra.Total);
            Assert.Equal(dto.PaymentMethodType, compra.PaymentMethodType);
        }
    }
}