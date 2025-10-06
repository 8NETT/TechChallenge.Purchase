using System;
using TechChallenge.Purchases.Core.Builders;
using TechChallenge.Purchases.Core.Entity;
using TechChallenge.Purchases.Core.Enum;
using TechChallenge.Purchases.Core.Exceptions;
using Xunit;

namespace TechChallenge.Purchases.Tests.Core.Builders
{
    public class CompraBuilderTests
    {
        [Fact]
        public void DeveConstruir_QuandoCompraValida()
        {
            // Arrange
            var jogoId = Guid.NewGuid();
            var builder = Compra.New();
            var data = DateTime.UtcNow;

            // Act
            var compra = builder
                .Id(1)
                .CompradorId(123)
                .JogoId(jogoId)
                .Valor(100.50m)
                .Desconto(10)
                .Total(90.45m)
                .CreatedAt(data)
                .PaymentMethodType(EPaymentMethodType.Credit)
                .Build();

            // Assert
            Assert.NotNull(compra);
            Assert.Equal(1, compra.Id);
            Assert.Equal(123, compra.CompradorId);
            Assert.Equal(jogoId, compra.JogoId);
            Assert.Equal(100.50m, compra.Valor);
            Assert.Equal(10, compra.Desconto);
            Assert.Equal(90.45m, compra.Total);
            Assert.Equal(data, compra.CreatedAt);
            Assert.Equal(EPaymentMethodType.Credit, compra.PaymentMethodType);
        }

        [Fact]
        public void DeveLancarExcecao_QuandoEstadoInvalido()
        {
            // Arrange
            var builder = Compra.New(); // Estado inválido, campos obrigatórios ausentes

            // Act & Assert
            var ex = Assert.Throws<EstadoInvalidoException>(() => builder.Build());
            Assert.Equal("Não é possível criar uma compra em um estado inválido.", ex.Message);
        }

        [Fact]
        public void DeveRetornarResultadoInvalido_QuandoEstadoInvalido()
        {
            // Arrange
            var builder = Compra.New().Valor(10); // Ainda inválido, outros campos ausentes

            // Act
            var result = builder.Validate();

            // Assert
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void DeveRetornarResultadoValido_QuandoEstadoValido()
        {
            // Arrange
            var jogoId = Guid.NewGuid();
            var builder = Compra.New()
                .CompradorId(1)
                .JogoId(jogoId)
                .Valor(10)
                .Total(10)
                .CreatedAt(DateTime.UtcNow);

            // Act
            var result = builder.Validate();

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }
    }
}
