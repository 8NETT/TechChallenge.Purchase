namespace TechChallenge.Purchases.Infrastructure.Messaging;

public record OrderPlacedEvent(
    int? OrderId,
    int UserId,
    int JogoId,
    decimal UnitPrice,
    decimal Total,
    DateTime OccurredAtUtc
    );