using ContratoApp.Dominio.Enums;
using ContratoApp.Funcionalidades.OrdemServicoItem.Compartilhado.Response;
using ContratoApp.Funcionalidades.OrdemServicoItem.Compartilhado.Rotas;
using ContratoApp.Infra.Endpoints.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace ContratoApp.Funcionalidades.OrdemServicoItem.ObterPorId;

public sealed class ObterOrdemServicoItemPorIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(RotaConsts.ObterPorId, HandleAsync)
            .WithTags("Ordem de Serviço Itens")
            .WithName("ObterOrdemServicoItemPorId")
            .WithSummary("Obtém item por chave composta.")
            .Produces<OrdemServicoItemResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }

    public static async Task<IResult> HandleAsync(
        [FromQuery] Guid idOrdemServico,
        [FromQuery] ETipoOrdemServicoItem tipo,
        [FromQuery] string descricao,
        IObterOrdemServicoItemPorIdHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(idOrdemServico, tipo, descricao, cancellationToken).ConfigureAwait(false);

        if (result.IsFailed)
            return Results.NotFound(new { erros = result.Errors.Select(x => x.Message).ToArray() });

        return Results.Ok(result.Value);
    }
}
