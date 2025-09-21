using TechChallenge.Purchases.Application.DTOs;
using TechChallenge.Purchases.Core.Enum;

namespace TechChallenge.Purchases.Application.Record;

public record CompraInput(
    int JogoId,
    decimal Valor,
    int Desconto,
    EPaymentMethodType PaymentMethod,
    PaymentMethodDTO? Payment
    );