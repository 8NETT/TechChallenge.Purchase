using TechChallenge.Purchases.Core.Entity;
using Xunit;

namespace TechChallenge.Purchases.Tests.Core.Entity
{
    public class CompraTests
    {
        [Fact]
        public void DeveMarcarComoEstornada_QuandoMetodoChamado()
        {
            // Arrange
            var compra = new Compra();

            // Act
            compra.MarcarEstornada();

            // Assert
            Assert.True(compra.Estornada);
        }

        [Fact]
        public void DeveIniciarComEstornadaFalso_QuandoNovaCompra()
        {
            // Arrange
            var compra = new Compra();

            // Assert
            Assert.False(compra.Estornada);
        }
    }
}