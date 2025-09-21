using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TechChallenge.Purchases.Core.Auth;

namespace TechChallenge.Purchases.Infrastructure.Auth;

public sealed class UserContext(IHttpContextAccessor accessor) : IUserContext
{
    public int GetUserId()
    {
        var user = accessor.HttpContext?.User
                   ?? throw new UnauthorizedAccessException("Usuário não autenticado.");

        var idStr =
            user.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
            user.FindFirst("sub")?.Value ??
            user.FindFirst("uid")?.Value ??
            throw new UnauthorizedAccessException("Claim de usuário ausente.");
        
        return int.Parse(idStr);
    }
}