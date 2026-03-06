using ContratoApp.Funcionalidades.OrdemServico.Compartilhado.Response;
using ContratoApp.Funcionalidades.OrdemServico.Compartilhado.Rotas;
using ContratoApp.Infra.Endpoints.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace ContratoApp.Funcionalidades.OrdemServico.ObterPorPeriodo;

public sealed class ObterOrdensServicoPorPeriodoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(RotaConsts.ObterPorPeriodo, HandleAsync)
            .WithTags("Ordens de Serviço")
            .WithName("ObterOrdensServicoPorPeriodo")
            .WithSummary("Lista ordens de serviço por período de abertura.")
            .Produces<IReadOnlyCollection<OrdemServicoResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
    }

    public static async Task<IResult> HandleAsync(
        [FromQuery] DateTime inicio,
        [FromQuery] DateTime fim,
        IObterOrdensServicoPorPeriodoHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(inicio, fim, cancellationToken).ConfigureAwait(false);

        if (result.IsFailed)
            return Results.BadRequest(new { erros = result.Errors.Select(x => x.Message).ToArray() });

        return Results.Ok(result.Value);
    }
}
