using Ardalis.Result;
using Microsoft.Extensions.Logging;
using TechChallenge.Purchases.Application.Contracts;
using TechChallenge.Purchases.Application.DTOs;
using TechChallenge.Purchases.Application.Mappers;
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
        public async Task<Result<int>> ComprarAsync(CompraDTO dto)
        {
            try
            {
                var userId = _userContext.GetUserId();
                
                var desconto = Math.Clamp(dto.Desconto, 0, 100);
                var total = Math.Round(dto.Valor * (100 - desconto) / 100m, 2);
                
                dto.UsuarioId = userId;
                dto.Desconto = desconto;
                dto.Total = total;
                
                var pm = dto.PaymentMethod;
                
                if (dto.PaymentMethodType is EPaymentMethodType.Credit or EPaymentMethodType.Debit)
                    if (pm != null)
                        CreditCardValidator.Validate(pm, dto.PaymentMethodType);

                var compra = dto.ToEntity();

                _repo.CompraRepository.Cadastrar(compra);
                await _repo.CommitAsync();
                
                var createdEvt = new CompraCriadaEvent(
                    CompraId: compra.Id,
                    UserId: userId,
                    JogoId: dto.JogoId,
                    Valor: dto.Valor,
                    Desconto: desconto,
                    Total: total,
                    PaymentMethodType: dto.PaymentMethodType,
                    OccurredAtUtc: DateTime.UtcNow);

                var orderEvt = new OrderPlacedEvent(
                    OrderId: compra.Id,
                    UserId: userId,
                    JogoId: dto.JogoId,
                    UnitPrice: dto.Valor,
                    Total: total,
                    OccurredAtUtc: DateTime.UtcNow);

                await _events.PublishAsync("CompraCriada", createdEvt);
                await _events.PublishAsync("OrderPlaced", orderEvt);

                return Result.Success(compra.Id);
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