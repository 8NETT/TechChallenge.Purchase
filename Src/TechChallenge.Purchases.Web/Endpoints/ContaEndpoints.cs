using System.Net;
using Ardalis.Result;
using TechChallenge.Purchases.Application.Contracts;
using TechChallenge.Purchases.Application.DTOs;
using TechChallenge.Purchases.Web.Security;

namespace TechChallenge.Purchases.Web.Endpoints;

public static class ContaEndpoints
{
        public static RouteGroupBuilder MapContaEndpoints(this IEndpointRouteBuilder app)
    {
        var conta = app.MapGroup("/api/conta");

        // POST: /api/conta/login
        conta.MapPost("/login", async (LoginDTO dto, IUsuarioService usuarioService, IJwtService jwtService) =>
        {
            try
            {
                var result = await usuarioService.LoginAsync(dto);

                if (result.IsInvalid())
                    return Results.BadRequest(result.Errors);
                if (result.IsUnauthorized())
                    return Results.Unauthorized();

                var token = jwtService.GenerateToken(result.Value);
                return Results.Ok(token);
            }
            catch (Exception e)
            {
                return Results.BadRequest(new { error = e.Message });
            }
        })
        .WithOpenApi(op => new(op)
        {
            OperationId = "LoginAsync",
            Summary = "Usuário logado com sucesso",
            Description = "Autentica um usuário e retorna o JWT"
        })
        .Produces((int)HttpStatusCode.OK)
        .Produces((int)HttpStatusCode.Unauthorized)
        .Produces((int)HttpStatusCode.BadRequest);

        return conta;
    }
}