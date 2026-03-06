using ContratoApp.Funcionalidades.OrdemServicoItem.Compartilhado.Response;
using ContratoApp.Funcionalidades.OrdemServicoItem.Compartilhado.Rotas;
using ContratoApp.Infra.Endpoints.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace ContratoApp.Funcionalidades.OrdemServicoItem.ObterPorOrdemServico;

public sealed class ObterItensPorOrdemServicoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(RotaConsts.ObterPorOrdemServico, HandleAsync)
            .WithTags("Ordem de Serviço Itens")
            .WithName("ObterItensPorOrdemServico")
            .WithSummary("Lista itens de uma ordem de serviço.")
            .Produces<IReadOnlyCollection<OrdemServicoItemResponse>>(StatusCodes.Status200OK);
    }

    public static async Task<IResult> HandleAsync(
        [FromRoute] Guid idOrdemServico,
        IObterItensPorOrdemServicoHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(idOrdemServico, cancellationToken).ConfigureAwait(false);
        return Results.Ok(result.Value);
    }
}
