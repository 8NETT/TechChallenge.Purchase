using System.Text.RegularExpressions;
using TechChallenge.Purchases.Application.DTOs;
using TechChallenge.Purchases.Core.Enum;

namespace TechChallenge.Purchases.Application.Validation;

public static class CreditCardValidator
{
    public static void Validate(PaymentMethodDTO pm, EPaymentMethodType pmType)
    {
        if (pmType is EPaymentMethodType.Pix) return;
        
        if (string.IsNullOrWhiteSpace(pm.CardHolderName) ||
            string.IsNullOrWhiteSpace(pm.CardNumber) ||
            string.IsNullOrWhiteSpace(pm.CVV) ||
            pm.ExpirationDateMonth is null ||
            pm.ExpirationDateYear is null)
            throw new ArgumentException("Dados do cartão incompletos.");

        var digits = Regex.Replace(pm.CardNumber!, @"\D", "");
        if (!Luhn(digits)) throw new ArgumentException("Número de cartão inválido.");

        if (pm.CVV!.Length is < 3 or > 4) throw new ArgumentException("CVV inválido.");
        
        var month = pm.ExpirationDateMonth!.Value;
        var year = pm.ExpirationDateYear!.Value;
        if (month < 1 || month > 12) throw new ArgumentException("Mês de expiração inválido.");
        
        var lastDay = DateTime.DaysInMonth(year, month);
        var expiry = new DateTime(year, month, lastDay, 23, 59, 59, DateTimeKind.Utc);
        if (DateTime.UtcNow > expiry) throw new ArgumentException("Cartão expirado.");
    }

    private static bool Luhn(string digits)
    {
        int sum = 0; bool alternate = false;
        for (int i = digits.Length - 1; i >= 0; i--)
        {
            int n = digits[i] - '0';
            if (alternate) { n *= 2; if (n > 9) n -= 9; }
            sum += n; alternate = !alternate;
        }
        return sum % 10 == 0;
    }
}