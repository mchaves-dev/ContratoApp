using ContratoApp.Funcionalidades.OrdemServico.Compartilhado.Response;
using ContratoApp.Funcionalidades.OrdemServico.Compartilhado.Rotas;
using ContratoApp.Infra.Endpoints.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace ContratoApp.Funcionalidades.OrdemServico.Atualizar;

public sealed class AtualizarOrdemServicoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(RotaConsts.Atualizar, HandleAsync)
            .WithTags("Ordens de Serviço")
            .WithName("AtualizarOrdemServico")
            .WithSummary("Atualiza uma ordem de serviço existente.")
            .Produces<OrdemServicoResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
    }

    public static async Task<IResult> HandleAsync(
        [FromRoute] Guid id,
        [FromBody] AtualizarOrdemServicoRequest request,
        IAtualizarOrdemServicoHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(id, request, cancellationToken).ConfigureAwait(false);

        if (result.IsFailed)
        {
            if (result.Errors.Any(x => x.Message.Contains("não encontrada", StringComparison.OrdinalIgnoreCase))
                || result.Errors.Any(x => x.Message.Contains("não encontrado", StringComparison.OrdinalIgnoreCase)))
                return Results.NotFound(new { erros = result.Errors.Select(x => x.Message).ToArray() });

            return Results.BadRequest(new { erros = result.Errors.Select(x => x.Message).ToArray() });
        }

        return Results.Ok(result.Value);
    }
}
