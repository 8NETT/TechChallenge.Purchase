using System;
using TechChallenge.Purchases.Application.DTOs;
using TechChallenge.Purchases.Application.Validation;
using TechChallenge.Purchases.Core.Enum;
using Xunit;

namespace TechChallenge.Purchases.Tests.Application.Validation
{
    public class CreditCardValidatorTests
    {
        private PaymentMethodDTO CriarMetodoDePagamentoValido() => new()
        {
            CardHolderName = "John Doe",
            CardNumber = "49927398716", // Luhn válido
            CVV = "123",
            ExpirationDateMonth = 12,
            ExpirationDateYear = DateTime.Now.Year + 1
        };

        [Fact]
        public void DeveSerValido_QuandoPagamentoEhPix()
        {
            var paymentMethod = new PaymentMethodDTO();
            var exception = Record.Exception(() => CreditCardValidator.Validate(paymentMethod, EPaymentMethodType.Pix));
            Assert.Null(exception);
        }

        [Theory]
        [InlineData(null, "49927398716", "123", 12, 2030)]
        [InlineData("John Doe", null, "123", 12, 2030)]
        [InlineData("John Doe", "49927398716", null, 12, 2030)]
        [InlineData("John Doe", "49927398716", "123", null, 2030)]
        [InlineData("John Doe", "49927398716", "123", 12, null)]
        public void DeveLancarExcecao_QuandoCampoObrigatorioDoCartaoNulo(string name, string number, string cvv, int? month, int? year)
        {
            var paymentMethod = new PaymentMethodDTO
            {
                CardHolderName = name,
                CardNumber = number,
                CVV = cvv,
                ExpirationDateMonth = month,
                ExpirationDateYear = year
            };
            var ex = Assert.Throws<ArgumentException>(() => CreditCardValidator.Validate(paymentMethod, EPaymentMethodType.Credit));
            Assert.Equal("Dados do cartão incompletos.", ex.Message);
        }

        [Fact]
        public void DeveLancarExcecao_QuandoNumeroDoCartaoInvalido()
        {
            var paymentMethod = CriarMetodoDePagamentoValido();
            paymentMethod.CardNumber = "1234567890123456"; // Luhn inválido
            var ex = Assert.Throws<ArgumentException>(() => CreditCardValidator.Validate(paymentMethod, EPaymentMethodType.Credit));
            Assert.Equal("Número de cartão inválido.", ex.Message);
        }

        [Theory]
        [InlineData("12")]
        [InlineData("12345")]
        public void DeveLancarExcecao_QuandoCvvInvalido(string cvv)
        {
            var paymentMethod = CriarMetodoDePagamentoValido();
            paymentMethod.CVV = cvv;
            var ex = Assert.Throws<ArgumentException>(() => CreditCardValidator.Validate(paymentMethod, EPaymentMethodType.Credit));
            Assert.Equal("CVV inválido.", ex.Message);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("1234")]
        public void DeveSerValido_QuandoCvvValido(string cvv)
        {
            var paymentMethod = CriarMetodoDePagamentoValido();
            paymentMethod.CVV = cvv;
            var exception = Record.Exception(() => CreditCardValidator.Validate(paymentMethod, EPaymentMethodType.Credit));
            Assert.Null(exception);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(13)]
        public void DeveLancarExcecao_QuandoMesDeExpiracaoInvalido(int month)
        {
            var paymentMethod = CriarMetodoDePagamentoValido();
            paymentMethod.ExpirationDateMonth = month;
            var ex = Assert.Throws<ArgumentException>(() => CreditCardValidator.Validate(paymentMethod, EPaymentMethodType.Credit));
            Assert.Equal("Mês de expiração inválido.", ex.Message);
        }

        [Fact]
        public void DeveLancarExcecao_QuandoCartaoExpirado()
        {
            var paymentMethod = CriarMetodoDePagamentoValido();
            paymentMethod.ExpirationDateYear = DateTime.Now.Year - 1;
            var ex = Assert.Throws<ArgumentException>(() => CreditCardValidator.Validate(paymentMethod, EPaymentMethodType.Credit));
            Assert.Equal("Cartão expirado.", ex.Message);
        }

        [Fact]
        public void DeveSerValido_QuandoCartaoValido()
        {
            var paymentMethod = CriarMetodoDePagamentoValido();
            var exception = Record.Exception(() => CreditCardValidator.Validate(paymentMethod, EPaymentMethodType.Credit));
            Assert.Null(exception);
        }
    }
}
