namespace TechChallenge.Purchases.Infrastructure.Messaging;

public record CompraEstornadaEvent(
    int CompraId,
    int UserId,
    Guid JogoId,
    DateTime OccurredAtUtc
    );