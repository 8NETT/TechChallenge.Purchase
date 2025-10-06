using TechChallenge.Purchases.Core.Enum;

namespace TechChallenge.Purchases.Infrastructure.Messaging;

public record CompraCriadaEvent(
    int? CompraId,
    int UserId,
    Guid JogoId,
    decimal Valor,
    int Desconto,
    decimal? Total,
    EPaymentMethodType PaymentMethodType,
    DateTime OccurredAtUtc
    );