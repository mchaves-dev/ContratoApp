using ContratoApp.Funcionalidades.Contrato.Compartilhado.Response;
using ContratoApp.Funcionalidades.Contrato.Compartilhado.Rotas;
using ContratoApp.Infra.Endpoints.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace ContratoApp.Funcionalidades.Contrato.ObterPorId;

public sealed class ObterContratoPorIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(RotaConsts.ObterPorId, HandleAsync)
            .WithTags("Contratos")
            .WithName("ObterContratoPorId")
            .WithSummary("Obtém contrato por id.")
            .Produces<ContratoResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }

    public static async Task<IResult> HandleAsync(
        [FromRoute] Guid id,
        IObterContratoPorIdHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(id, cancellationToken).ConfigureAwait(false);
        if (result.IsFailed)
            return Results.NotFound(new { erros = result.Errors.Select(x => x.Message).ToArray() });

        return Results.Ok(result.Value);
    }
}
