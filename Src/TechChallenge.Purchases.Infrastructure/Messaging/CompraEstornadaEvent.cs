namespace TechChallenge.Purchases.Infrastructure.Messaging;

public record CompraEstornadaEvent(
    int CompraId,
    int UserId,
    int JogoId,
    DateTime OccurredAtUtc
    );