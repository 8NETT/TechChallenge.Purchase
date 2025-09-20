using TechChallenge.Purchases.Application.DTOs;

namespace TechChallenge.Purchases.Web.Security;

public interface IJwtService
{
    string GenerateToken(UsuarioDTO usuario);
}