using ContratoApp.Funcionalidades.OrdemServicoItem.Compartilhado.Response;
using ContratoApp.Funcionalidades.OrdemServicoItem.Compartilhado.Rotas;
using ContratoApp.Infra.Endpoints.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace ContratoApp.Funcionalidades.OrdemServicoItem.AtualizarItem;

public sealed class AtualizarOrdemServicoItemEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(RotaConsts.Atualizar, HandleAsync)
            .WithTags("Ordem de Serviço Itens")
            .WithName("AtualizarOrdemServicoItem")
            .WithSummary("Atualiza item da ordem de serviço.")
            .Produces<OrdemServicoItemResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest);
    }

    public static async Task<IResult> HandleAsync(
        [FromBody] AtualizarOrdemServicoItemRequest request,
        IAtualizarOrdemServicoItemHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(request, cancellationToken).ConfigureAwait(false);

        if (result.IsFailed)
        {
            if (result.Errors.Any(x => x.Message.Contains("não encontrado", StringComparison.OrdinalIgnoreCase)))
                return Results.NotFound(new { erros = result.Errors.Select(x => x.Message).ToArray() });

            return Results.BadRequest(new { erros = result.Errors.Select(x => x.Message).ToArray() });
        }

        return Results.Ok(result.Value);
    }
}
