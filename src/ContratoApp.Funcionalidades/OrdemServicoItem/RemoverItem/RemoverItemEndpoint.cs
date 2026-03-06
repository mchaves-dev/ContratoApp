using ContratoApp.Dominio.Enums;
using ContratoApp.Funcionalidades.OrdemServicoItem.Compartilhado.Rotas;
using ContratoApp.Infra.Endpoints.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace ContratoApp.Funcionalidades.OrdemServicoItem.RemoverItem;

public sealed class RemoverOrdemServicoItemEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(RotaConsts.Remover, HandleAsync)
            .WithTags("Ordem de Serviço Itens")
            .WithName("RemoverOrdemServicoItem")
            .WithSummary("Remove item da ordem de serviço.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
    }

    public static async Task<IResult> HandleAsync(
        [FromQuery] Guid idOrdemServico,
        [FromQuery] ETipoOrdemServicoItem tipo,
        [FromQuery] string descricao,
        IRemoverOrdemServicoItemHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(idOrdemServico, tipo, descricao, cancellationToken).ConfigureAwait(false);

        if (result.IsFailed)
            return Results.NotFound(new { erros = result.Errors.Select(x => x.Message).ToArray() });

        return Results.NoContent();
    }
}
