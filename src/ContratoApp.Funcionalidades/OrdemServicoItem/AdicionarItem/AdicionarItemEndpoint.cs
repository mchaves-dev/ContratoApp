using ContratoApp.Funcionalidades.OrdemServicoItem.Compartilhado.Response;
using ContratoApp.Funcionalidades.OrdemServicoItem.Compartilhado.Rotas;
using ContratoApp.Infra.Endpoints.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace ContratoApp.Funcionalidades.OrdemServicoItem.AdicionarItem;

public sealed class AdicionarOrdemServicoItemEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(RotaConsts.Adicionar, HandleAsync)
            .WithTags("Ordem de Serviço Itens")
            .WithName("AdicionarOrdemServicoItem")
            .WithSummary("Adiciona item na ordem de serviço.")
            .Produces<OrdemServicoItemResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
    }

    public static async Task<IResult> HandleAsync(
        [FromBody] AdicionarOrdemServicoItemRequest request,
        IAdicionarOrdemServicoItemHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(request, cancellationToken).ConfigureAwait(false);

        if (result.IsFailed)
        {
            if (result.Errors.Any(x => x.Message.Contains("não encontrada", StringComparison.OrdinalIgnoreCase)))
                return Results.NotFound(new { erros = result.Errors.Select(x => x.Message).ToArray() });

            return Results.BadRequest(new { erros = result.Errors.Select(x => x.Message).ToArray() });
        }

        return Results.Created(RotaConsts.RotaBase, result.Value);
    }
}
