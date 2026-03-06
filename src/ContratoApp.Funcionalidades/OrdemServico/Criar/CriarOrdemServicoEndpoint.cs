using ContratoApp.Funcionalidades.OrdemServico.Compartilhado.Response;
using ContratoApp.Funcionalidades.OrdemServico.Compartilhado.Rotas;
using ContratoApp.Infra.Endpoints.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace ContratoApp.Funcionalidades.OrdemServico.Criar;

public sealed class CriarOrdemServicoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(RotaConsts.Criar, HandleAsync)
            .WithTags("Ordens de Serviço")
            .WithName("CriarOrdemServico")
            .WithSummary("Cria uma nova ordem de serviço.")
            .Produces<OrdemServicoResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
    }

    public static async Task<IResult> HandleAsync(
        [FromBody] CriarOrdemServicoRequest request,
        ICriarOrdemServicoHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(request, cancellationToken).ConfigureAwait(false);

        if (result.IsFailed)
        {
            if (result.Errors.Any(x => x.Message.Contains("não encontrado", StringComparison.OrdinalIgnoreCase)))
                return Results.NotFound(new { erros = result.Errors.Select(x => x.Message).ToArray() });

            return Results.BadRequest(new { erros = result.Errors.Select(x => x.Message).ToArray() });
        }

        return Results.Created($"{RotaConsts.RotaBase}/{result.Value.Id}", result.Value);
    }
}
