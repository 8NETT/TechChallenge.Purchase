using System;
using TechChallenge.Purchases.Core.Extensions;
using Xunit;

namespace TechChallenge.Purchases.Tests.Core.Extensions
{
    public class FunctionalExtensionsTests
    {
        private class ObjetoDeTeste
        {
            public int Valor { get; set; }
        }

        [Fact]
        public void DeveAplicarFuncao_QuandoMapeando()
        {
            // Arrange
            var source = "hello";
            Func<string, int> function = s => s.Length;

            // Act
            var result = source.Map(function);

            // Assert
            Assert.Equal(5, result);
        }

        [Fact]
        public void DeveExecutarAcaoERetornarObjetoOriginal_QuandoUsandoTee()
        {
            // Arrange
            var source = new ObjetoDeTeste { Valor = 10 };
            Action<ObjetoDeTeste> action = obj => obj.Valor = 20;

            // Act
            var result = source.Tee(action);

            // Assert
            Assert.Equal(20, source.Valor); // Verifica se a ação foi executada
            Assert.Same(source, result);   // Verifica se o objeto original foi retornado
        }
    }
}