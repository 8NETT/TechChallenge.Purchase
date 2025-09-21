using Ardalis.Result;
using Microsoft.Extensions.Logging;
using TechChallenge.Purchases.Application.Contracts;
using TechChallenge.Purchases.Application.DTOs;
using TechChallenge.Purchases.Application.Mappers;
using TechChallenge.Purchases.Application.Record;
using TechChallenge.Purchases.Application.Validation;
using TechChallenge.Purchases.Core.Auth;
using TechChallenge.Purchases.Core.Entity;
using TechChallenge.Purchases.Core.Enum;
using TechChallenge.Purchases.Core.EventHub;
using TechChallenge.Purchases.Core.Repository;
using TechChallenge.Purchases.Infrastructure.Messaging;

namespace TechChallenge.Purchases.Application.Services
{
    public class CompraService(
        IUnitOfWork _repo,
        IUserContext _userContext,
        IEventHubClient _events,
        ILogger<CompraService> _log) : ICompraService
    {
        public async Task<Result<int>> ComprarAsync(CompraInput input)
        {
            try
            {
                var userId = _userContext.GetUserId();
                
                var desconto = Math.Clamp(input.Desconto, 0, 100);
                var total = Math.Round(input.Valor * (100 - desconto) / 100m, 2);

                var pm = input.Payment;
                
                if (input.PaymentMethod is EPaymentMethodType.Credit or EPaymentMethodType.Debit)
                    if (pm != null)
                        CreditCardValidator.Validate(pm, input.PaymentMethod);

                var compra = new Compra
                {
                    CompradorId = userId,
                    JogoId = input.JogoId,
                    Valor = input.Valor,
                    Desconto = desconto,
                    Total = total,
                    PaymentMethodType = input.PaymentMethod
                };

                var compraId = _repo.CompraRepository.Cadastrar(compra);

                // Eventos: CompraCriada (para usuários/jogos) + OrderPlaced (para pagamentos)
                var createdEvt = new CompraCriadaEvent(
                    CompraId: compraId,
                    UserId: userId,
                    JogoId: input.JogoId,
                    Valor: input.Valor,
                    Desconto: desconto,
                    Total: total,
                    PaymentMethodType: input.PaymentMethod,
                    OccurredAtUtc: DateTime.UtcNow);

                var orderEvt = new OrderPlacedEvent(
                    OrderId: compraId,
                    UserId: userId,
                    JogoId: input.JogoId,
                    UnitPrice: input.Valor,
                    Total: total,
                    OccurredAtUtc: DateTime.UtcNow);

                await _events.PublishAsync("CompraCriada", createdEvt);
                await _events.PublishAsync("OrderPlaced", orderEvt);

                return Result.Success(compraId);
            }
            catch (UnauthorizedAccessException)
            {
                return Result.Unauthorized();
            }
            catch (ArgumentException ex)
            {
                _log.LogWarning(ex, "Falha de validação ao criar compra");
                return Result.Invalid();
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Erro ao criar compra");
                return Result.Error(ex.Message);
            }
        }

        public async Task<Result> EstornarAsync(int compraId)
        {
            try
            {
                var compra = await _repo.CompraRepository.ObterPorIdAsync(compraId);
                if (compra is null) return Result.NotFound();

                if (compra.Estornada) return Result.Success(); // idempotência

                compra.MarcarEstornada();
                
                _repo.CompraRepository.Alterar(compra);

                var evt = new CompraEstornadaEvent(
                    CompraId: compraId,
                    UserId: compra.CompradorId,
                    JogoId: compra.JogoId,
                    OccurredAtUtc: DateTime.UtcNow);

                await _events.PublishAsync("CompraEstornada", evt);

                return Result.Success();
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Erro ao estornar compra {CompraId}", compraId);
                return Result.Error(ex.Message);
            }
        }

        public void Dispose() => _repo.Dispose();
    }
}