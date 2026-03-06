using ContratoApp.Funcionalidades.Contrato.Compartilhado.Response;
using ContratoApp.Funcionalidades.Contrato.Compartilhado.Rotas;
using ContratoApp.Infra.Endpoints.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace ContratoApp.Funcionalidades.Contrato.ObterPorPeriodo;

public sealed class ObterContratosPorPeriodoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(RotaConsts.ObterPorPeriodo, HandleAsync)
            .WithTags("Contratos")
            .WithName("ObterContratosPorPeriodo")
            .WithSummary("Lista contratos por período de início.")
            .Produces<IReadOnlyCollection<ContratoResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
    }

    public static async Task<IResult> HandleAsync(
        [FromQuery] DateTime inicio,
        [FromQuery] DateTime fim,
        IObterContratosPorPeriodoHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(inicio, fim, cancellationToken).ConfigureAwait(false);

        if (result.IsFailed)
            return Results.BadRequest(new { erros = result.Errors.Select(x => x.Message).ToArray() });

        return Results.Ok(result.Value);
    }
}
