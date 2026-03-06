using ContratoApp.Funcionalidades.Clientes.Compartilhado.Rotas;
using ContratoApp.Infra.Endpoints.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace ContratoApp.Funcionalidades.Clientes.Criar;

public sealed class CriarClienteEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(RotaConsts.Criar, HandleAsync)
            .WithTags("Clientes")
            .WithName("CriarCliente")
            .WithSummary("Cria um novo cliente.")
            .WithDescription("Cria um novo cliente com as informações fornecidas.")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);
    }

    public static async Task<IResult> HandleAsync(
        [FromBody] CriarClienteRequest request,
        ICriarClienteHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(request, cancellationToken).ConfigureAwait(false);

        if (result.IsFailed)
            return Results.BadRequest(new { erros = result.Errors.Select(x => x.Message).ToArray() });

        var response = result.Value;
        return Results.Created($"{RotaConsts.RotaBase}/{response.Id}", response);
    }
}
