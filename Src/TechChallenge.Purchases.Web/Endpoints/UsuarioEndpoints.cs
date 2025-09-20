using System.Net;
using Ardalis.Result;
using TechChallenge.Purchases.Application.Contracts;
using TechChallenge.Purchases.Application.DTOs;
using Swashbuckle.AspNetCore.Annotations;

namespace TechChallenge.Purchases.Web.Endpoints;

public static class UsuarioEndpoints
{
    public static RouteGroupBuilder MapUsuarioEndpoints(this IEndpointRouteBuilder app)
    {
        var usuarios = app.MapGroup("/usuario")
            .RequireAuthorization("Administrador");

        usuarios.MapGet("/", async (IUsuarioService usuarioService) =>
            {
                try
                {
                    return Results.Ok(await usuarioService.ObterTodosAsync());
                }
                catch (Exception e)
                {
                    return Results.BadRequest(new { error = e.Message });
                }
            })
            .WithOpenApi(op => new(op)
            {
                OperationId = "GetUsuarioAsync"
            })
            .Produces((int)HttpStatusCode.OK)
            .Produces((int)HttpStatusCode.BadRequest);

        usuarios.MapGet("/{id:int}", async (int id, IUsuarioService usuarioService) =>
            {
                try
                {
                    var result = await usuarioService.ObterPorIdAsync(id);
                    if (result.IsNotFound())
                        return Results.NotFound(result.Errors);

                    return Results.Ok(result.Value);
                }
                catch (Exception e)
                {
                    return Results.BadRequest(new { error = e.Message });
                }
            })
            .WithOpenApi(op => new(op)
            {
                OperationId = "GetUsuarioPorIdAsync"
            })
            .Produces((int)HttpStatusCode.OK)
            .Produces((int)HttpStatusCode.NotFound)
            .Produces((int)HttpStatusCode.BadRequest);

        usuarios.MapPost("/", async (CadastrarUsuarioDTO dto, IUsuarioService usuarioService) =>
            {
                try
                {
                    var result = await usuarioService.CadastrarAsync(dto);

                    if (result.IsInvalid())
                        return Results.BadRequest(result.Errors);
                    if (result.IsConflict())
                        return Results.Conflict(result.Errors);

                    return Results.Ok(result.Value);
                }
                catch (Exception e)
                {
                    return Results.BadRequest(new { error = e.Message });
                }
            })
            .WithOpenApi(op => new(op)
            {
                OperationId = "PostUsuarioAsync"
            })
            .Produces((int)HttpStatusCode.OK)
            .Produces((int)HttpStatusCode.Conflict)
            .Produces((int)HttpStatusCode.BadRequest);

        usuarios.MapPut("/", async (AlterarUsuarioDTO dto, IUsuarioService usuarioService) =>
            {
                try
                {
                    var result = await usuarioService.AlterarAsync(dto);

                    if (result.IsInvalid())
                        return Results.BadRequest(result.Errors);
                    if (result.IsNotFound())
                        return Results.NotFound(result.Errors);
                    if (result.IsConflict())
                        return Results.Conflict(result.Errors);

                    return Results.Ok(result.Value);
                }
                catch (Exception e)
                {
                    return Results.BadRequest(new { error = e.Message });
                }
            })
            .WithOpenApi(op => new(op)
            {
                OperationId = "PutUsuarioAsync"
            })
            .Produces((int)HttpStatusCode.OK)
            .Produces((int)HttpStatusCode.NotFound)
            .Produces((int)HttpStatusCode.Conflict)
            .Produces((int)HttpStatusCode.BadRequest);

        usuarios.MapDelete("/{id:int}", async (int id, IUsuarioService usuarioService) =>
            {
                try
                {
                    var result = await usuarioService.DeletarAsync(id);

                    if (result.IsNotFound())
                        return Results.NotFound(result.Errors);

                    return Results.Ok();
                }
                catch (Exception e)
                {
                    return Results.BadRequest(new { error = e.Message });
                }
            })
            .WithOpenApi(op => new(op)
            {
                OperationId = "DeleteUsuarioAsync"
            })
            .Produces((int)HttpStatusCode.OK)
            .Produces((int)HttpStatusCode.NotFound)
            .Produces((int)HttpStatusCode.BadRequest);

        return usuarios;
    }
}