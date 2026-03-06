using ContratoApp.Funcionalidades.Clientes.Compartilhado.Response;
using ContratoApp.Funcionalidades.Clientes.Compartilhado.Rotas;
using ContratoApp.Infra.Endpoints.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace ContratoApp.Funcionalidades.Clientes.ObterPorId;

public sealed class ObterClientePorIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(RotaConsts.ObterPorId, HandleAsync)
            .WithTags("Clientes")
            .WithName("ObterClientePorId")
            .WithSummary("Obtém um cliente pelo identificador.")
            .Produces<ClienteResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }

    public static async Task<IResult> HandleAsync(
        [FromRoute] Guid id,
        IObterClientePorIdHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(id, cancellationToken).ConfigureAwait(false);

        if (result.IsFailed)
            return Results.NotFound(new { erros = result.Errors.Select(x => x.Message).ToArray() });

        return Results.Ok(result.Value);
    }
}
