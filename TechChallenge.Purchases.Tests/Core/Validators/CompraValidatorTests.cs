using System;
using FluentValidation.TestHelper;
using TechChallenge.Purchases.Core.Entity;
using TechChallenge.Purchases.Core.Validators;
using Xunit;

namespace TechChallenge.Purchases.Tests.Core.Validators
{
    public class CompraValidatorTests
    {
        private readonly CompraValidator _validator;

        public CompraValidatorTests()
        {
            _validator = new CompraValidator();
        }

        private Compra CriarCompraValida() => new Compra
        {
            CreatedAt = DateTime.UtcNow,
            JogoId = Guid.NewGuid(),
            CompradorId = 1,
            Valor = 100,
            Desconto = 10,
            Total = 90
        };

        [Fact]
        public void DeveTerErro_QuandoJogoIdVazio()
        {
            var model = CriarCompraValida();
            model.JogoId = Guid.Empty;
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(c => c.JogoId);
        }

        [Fact]
        public void DeveTerErro_QuandoCompradorIdVazio()
        {
            var model = CriarCompraValida();
            model.CompradorId = 0;
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(c => c.CompradorId);
        }

        [Fact]
        public void DeveTerErro_QuandoValorNegativo()
        {
            var model = CriarCompraValida();
            model.Valor = -1;
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(c => c.Valor);
        }

        [Fact]
        public void DeveTerErro_QuandoDescontoNegativo()
        {
            var model = CriarCompraValida();
            model.Desconto = -1;
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(c => c.Desconto);
        }

        [Fact]
        public void DeveTerErro_QuandoDescontoMaiorQue100()
        {
            var model = CriarCompraValida();
            model.Desconto = 101;
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(c => c.Desconto);
        }

        [Fact]
        public void DeveTerErro_QuandoTotalNegativo()
        {
            var model = CriarCompraValida();
            model.Total = -1;
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(c => c.Total);
        }

        [Fact]
        public void NaoDeveTerErro_QuandoCompraValida()
        {
            var model = CriarCompraValida();
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(0, 50, 50)]   // Valor 0
        [InlineData(100, 0, 100)]  // Desconto 0
        [InlineData(100, 100, 0)]  // Desconto 100
        public void NaoDeveTerErro_QuandoValoresDeBordaSaoUsados(decimal valor, int desconto, decimal total)
        {
            var model = CriarCompraValida();
            model.Valor = valor;
            model.Desconto = desconto;
            model.Total = total;
            
            var result = _validator.TestValidate(model);
            
            result.ShouldNotHaveValidationErrorFor(c => c.Valor);
            result.ShouldNotHaveValidationErrorFor(c => c.Desconto);
            result.ShouldNotHaveValidationErrorFor(c => c.Total);
        }
    }
}
