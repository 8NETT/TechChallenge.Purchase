using System.Net;
using Ardalis.Result;
using TechChallenge.Purchases.Application.Contracts;
using TechChallenge.Purchases.Application.Record;
using TechChallenge.Purchases.Web.Security;

namespace TechChallenge.Purchases.Web.Endpoints;

public static class CompraEndpoints
{
    public static RouteGroupBuilder MapCompraEndpoints(this IEndpointRouteBuilder app)
    {
        var compra = app.MapGroup("/api/compra")
            .RequireAuthorization(); 
        
        // POST: /api/compra
        compra.MapPost("/", async (CompraInput input, ICompraService service) =>
            {
                var result = await service.ComprarAsync(input);

                if (result.IsInvalid()) return Results.BadRequest(result.ValidationErrors);
                if (result.IsConflict()) return Results.Conflict(result.Errors);
                if (result.IsUnauthorized()) return Results.Unauthorized();
                if (result.Status == ResultStatus.Error) return Results.BadRequest(new { error = result.Errors });

                return Results.Ok(new { compraId = result.Value });
            })
            .WithOpenApi(op => new(op)
            {
                OperationId = "PostCompraAsync",
                Summary = "Criar compra",
                Description = "Cria uma nova compra e publica eventos para pagamentos, usuários e jogos"
            })
            .Produces((int)HttpStatusCode.OK)
            .Produces((int)HttpStatusCode.Conflict)
            .Produces((int)HttpStatusCode.BadRequest)
            .Produces((int)HttpStatusCode.Unauthorized);

        // POST: /api/compra/estornar/{id}
        compra.MapPost("/estornar/{id:int}", async (int id, ICompraService service) =>
            {
                var result = await service.EstornarAsync(id);
                if (result.IsNotFound()) return Results.NotFound();
                if (result.IsError()) return Results.BadRequest(new { error = result.Errors });
                return Results.Ok();
            })
            .RequireAuthorization("Administrador")
            .WithOpenApi(op => new(op)
            {
                OperationId = "PostEstornoAsync",
                Summary = "Estornar compra",
                Description = "Estorna uma compra e publica evento para os serviços ouvintes"
            })
            .Produces((int)HttpStatusCode.OK)
            .Produces((int)HttpStatusCode.NotFound)
            .Produces((int)HttpStatusCode.BadRequest);

        return compra;
    }
}