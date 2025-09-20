using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TechChallenge.Purchases.Application.DTOs;

namespace TechChallenge.Purchases.Web.Security;

public class JwtService(string key, string issuer) : IJwtService
{
    public string GenerateToken(UsuarioDTO usuario)
    {
        var key1 = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(key1, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: issuer,
            claims: GenerateClaims(usuario),
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private Claim[] GenerateClaims(UsuarioDTO usuario)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            usuario.Profile ? new Claim(ClaimTypes.Role, "Administrador") : new Claim(ClaimTypes.Role, "Usuário")
        };

        return claims.ToArray();
    }
}