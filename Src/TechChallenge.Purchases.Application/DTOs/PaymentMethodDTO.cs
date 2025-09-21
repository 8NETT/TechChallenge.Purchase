namespace TechChallenge.Purchases.Application.DTOs;

public class PaymentMethodDTO
{
    public string? CardHolderName { get; set; }
    public string? CardNumber { get; set; }
    public int? ExpirationDateMonth { get; set; }
    public int? ExpirationDateYear { get; set; }
    public string? CVV { get; set; }
}