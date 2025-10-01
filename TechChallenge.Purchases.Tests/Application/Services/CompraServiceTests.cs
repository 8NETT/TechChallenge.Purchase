using System;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.Result;
using Microsoft.Extensions.Logging;
using Moq;
using TechChallenge.Purchases.Application.DTOs;
using TechChallenge.Purchases.Application.Services;
using TechChallenge.Purchases.Core.Auth;
using TechChallenge.Purchases.Core.Entity;
using TechChallenge.Purchases.Core.Enum;
using TechChallenge.Purchases.Core.EventHub;
using TechChallenge.Purchases.Core.Repository;
using TechChallenge.Purchases.Infrastructure.Messaging;
using Xunit;

namespace TechChallenge.Purchases.Tests.Application.Services
{
    // Helper para capturar argumentos no Moq
    public static class Capture
    {
        public static T With<T>(Capture<T> capture) where T : class
        {
            return It.Is<T>(t => capture.Set(t));
        }
    }

    public class Capture<T> where T : class
    {
        public T Value { get; private set; }

        public bool Set(T value)
        {
            Value = value;
            return true;
        }
    }

    public class CompraServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICompraRepository> _compraRepositoryMock;
        private readonly Mock<IUserContext> _userContextMock;
        private readonly Mock<IEventHubClient> _eventHubClientMock;
        private readonly Mock<ILogger<CompraService>> _loggerMock;
        private readonly CompraService _service;

        public CompraServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _compraRepositoryMock = new Mock<ICompraRepository>();
            _userContextMock = new Mock<IUserContext>();
            _eventHubClientMock = new Mock<IEventHubClient>();
            _loggerMock = new Mock<ILogger<CompraService>>();

            _unitOfWorkMock.Setup(uow => uow.CompraRepository).Returns(_compraRepositoryMock.Object);

            _service = new CompraService(
                _unitOfWorkMock.Object,
                _userContextMock.Object,
                _eventHubClientMock.Object,
                _loggerMock.Object);
        }

        private CompraDTO CriarCompraDTOValida(EPaymentMethodType paymentMethodType = EPaymentMethodType.Credit)
        {
            return new CompraDTO
            {
                JogoId = 1,
                Valor = 100,
                Desconto = 10,
                PaymentMethodType = paymentMethodType,
                PaymentMethod = paymentMethodType == EPaymentMethodType.Pix ? null : new PaymentMethodDTO
                {
                    CardHolderName = "Test User",
                    CardNumber = "49927398716", // Luhn Válido
                    ExpirationDateMonth = 12,
                    ExpirationDateYear = 2030,
                    CVV = "123"
                }
            };
        }

        [Fact]
        public async Task DeveSalvarCompraComDadosCorretos_QuandoCompraValida()
        {
            var dto = CriarCompraDTOValida();
            var userId = 123;
            var compraCapturada = new Capture<Compra>();
            _userContextMock.Setup(u => u.GetUserId()).Returns(userId);
            _compraRepositoryMock.Setup(r => r.Cadastrar(Capture.With(compraCapturada)));

            await _service.ComprarAsync(dto);

            _compraRepositoryMock.Verify(r => r.Cadastrar(It.IsAny<Compra>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
            var compra = compraCapturada.Value;
            Assert.Equal(userId, compra.CompradorId);
            Assert.Equal(10, compra.Desconto);
            Assert.Equal(90.00m, compra.Total);
        }

        [Fact]
        public async Task DevePublicarEventoCompraCriada_QuandoCompraValida()
        {
            var dto = CriarCompraDTOValida();
            var userId = 123;
            var eventoCapturado = new Capture<CompraCriadaEvent>();
            _userContextMock.Setup(u => u.GetUserId()).Returns(userId);
            _eventHubClientMock.Setup(e => e.PublishAsync("CompraCriada", Capture.With(eventoCapturado), default));

            await _service.ComprarAsync(dto);

            _eventHubClientMock.Verify(e => e.PublishAsync("CompraCriada", It.IsAny<CompraCriadaEvent>(), default), Times.Once);
            var evt = eventoCapturado.Value;
            Assert.Equal(userId, evt.UserId);
            Assert.Equal(dto.JogoId, evt.JogoId);
            Assert.Equal(90.00m, evt.Total);
        }

        [Fact]
        public async Task DevePublicarEventoOrderPlaced_QuandoCompraValida()
        {
            var dto = CriarCompraDTOValida();
            var userId = 123;
            var eventoCapturado = new Capture<OrderPlacedEvent>();
            _userContextMock.Setup(u => u.GetUserId()).Returns(userId);
            _eventHubClientMock.Setup(e => e.PublishAsync("OrderPlaced", Capture.With(eventoCapturado), default));

            await _service.ComprarAsync(dto);

            _eventHubClientMock.Verify(e => e.PublishAsync("OrderPlaced", It.IsAny<OrderPlacedEvent>(), default), Times.Once);
            var evt = eventoCapturado.Value;
            Assert.Equal(userId, evt.UserId);
            Assert.Equal(dto.JogoId, evt.JogoId);
            Assert.Equal(dto.Valor, evt.UnitPrice);
            Assert.Equal(90.00m, evt.Total);
        }

        [Fact]
        public async Task DeveRetornarSucesso_QuandoEstornoDeCompraValida()
        {
            var compra = Compra.New().CompradorId(123).JogoId(456).Valor(100).Total(100).Build();
            compra.Id = 1; // Set Id after build for test purposes
            var eventoCapturado = new Capture<CompraEstornadaEvent>();
            _compraRepositoryMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(compra);
            _eventHubClientMock.Setup(e => e.PublishAsync("CompraEstornada", Capture.With(eventoCapturado), default));

            var result = await _service.EstornarAsync(1);

            Assert.True(result.IsSuccess);
            Assert.True(compra.Estornada);
            _compraRepositoryMock.Verify(r => r.Alterar(compra), Times.Once);
            _eventHubClientMock.Verify(e => e.PublishAsync("CompraEstornada", It.IsAny<CompraEstornadaEvent>(), default), Times.Once);
            var evt = eventoCapturado.Value;
            Assert.Equal(1, evt.CompraId);
            Assert.Equal(123, evt.UserId);
            Assert.Equal(456, evt.JogoId);
        }

        [Fact]
        public async Task DeveRetornarSucesso_QuandoCompraComPix()
        {
            var dto = CriarCompraDTOValida(EPaymentMethodType.Pix);
            _userContextMock.Setup(u => u.GetUserId()).Returns(123);
            var result = await _service.ComprarAsync(dto);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task DeveRetornarInvalido_QuandoDadosDoCartaoInvalidos()
        {
            var dto = CriarCompraDTOValida();
            dto.PaymentMethod.CardNumber = "1234567890123456";
            _userContextMock.Setup(u => u.GetUserId()).Returns(123);
            var result = await _service.ComprarAsync(dto);
            Assert.Equal(ResultStatus.Invalid, result.Status);
        }

        [Fact]
        public async Task DeveRetornarNaoAutorizado_QuandoUsuarioNaoAutenticado()
        {
            var dto = CriarCompraDTOValida();
            _userContextMock.Setup(u => u.GetUserId()).Throws<UnauthorizedAccessException>();
            var result = await _service.ComprarAsync(dto);
            Assert.Equal(ResultStatus.Unauthorized, result.Status);
        }

        [Fact]
        public async Task DeveRetornarErro_QuandoRepositorioLancaExcecao()
        {
            var dto = CriarCompraDTOValida();
            _userContextMock.Setup(u => u.GetUserId()).Returns(123);
            _unitOfWorkMock.Setup(u => u.CommitAsync()).ThrowsAsync(new Exception("DB error"));
            var result = await _service.ComprarAsync(dto);
            Assert.Equal(ResultStatus.Error, result.Status);
        }

        [Fact]
        public async Task DeveRetornarNaoEncontrado_QuandoCompraParaEstornoNaoExiste()
        {
            _compraRepositoryMock.Setup(r => r.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync((Compra)null);
            var result = await _service.EstornarAsync(1);
            Assert.Equal(ResultStatus.NotFound, result.Status);
        }

        [Fact]
        public async Task DeveRetornarSucesso_QuandoCompraJaEstornada()
        {
            var compra = new Compra { Estornada = true };
            _compraRepositoryMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(compra);
            var result = await _service.EstornarAsync(1);
            Assert.True(result.IsSuccess);
            _compraRepositoryMock.Verify(r => r.Alterar(It.IsAny<Compra>()), Times.Never);
        }

        [Fact]
        public async Task DeveRetornarErro_QuandoRepositorioLancaExcecaoNoEstorno()
        {
            var compra = new Compra { Estornada = false };
            _compraRepositoryMock.Setup(r => r.ObterPorIdAsync(1)).ReturnsAsync(compra);
            _compraRepositoryMock.Setup(r => r.Alterar(It.IsAny<Compra>())).Throws(new Exception("DB error"));
            var result = await _service.EstornarAsync(1);
            Assert.Equal(ResultStatus.Error, result.Status);
        }
    }
}
