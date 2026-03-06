using ContratoApp.Funcionalidades.OrdemServico.Compartilhado.Response;
using ContratoApp.Funcionalidades.OrdemServico.Compartilhado.Rotas;
using ContratoApp.Infra.Endpoints.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace ContratoApp.Funcionalidades.OrdemServico.ObterPorId;

public sealed class ObterOrdemServicoPorIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(RotaConsts.ObterPorId, HandleAsync)
            .WithTags("Ordens de Serviço")
            .WithName("ObterOrdemServicoPorId")
            .WithSummary("Obtém uma ordem de serviço por id.")
            .Produces<OrdemServicoResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }

    public static async Task<IResult> HandleAsync(
        [FromRoute] Guid id,
        IObterOrdemServicoPorIdHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(id, cancellationToken).ConfigureAwait(false);

        if (result.IsFailed)
            return Results.NotFound(new { erros = result.Errors.Select(x => x.Message).ToArray() });

        return Results.Ok(result.Value);
    }
}
