using ContratoApp.Funcionalidades.Contrato.Compartilhado.Response;
using ContratoApp.Funcionalidades.Contrato.Compartilhado.Rotas;
using ContratoApp.Infra.Endpoints.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace ContratoApp.Funcionalidades.Contrato.Atualizar;

public sealed class AtualizarContratoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(RotaConsts.Atualizar, HandleAsync)
            .WithTags("Contratos")
            .WithName("AtualizarContrato")
            .WithSummary("Atualiza um contrato existente.")
            .Produces<ContratoResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest);
    }

    public static async Task<IResult> HandleAsync(
        [FromRoute] Guid id,
        [FromBody] AtualizarContratoRequest request,
        IAtualizarContratoHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(id, request, cancellationToken).ConfigureAwait(false);

        if (result.IsFailed)
        {
            if (result.Errors.Any(x => x.Message.Contains("não encontrado", StringComparison.OrdinalIgnoreCase)))
                return Results.NotFound(new { erros = result.Errors.Select(x => x.Message).ToArray() });

            return Results.BadRequest(new { erros = result.Errors.Select(x => x.Message).ToArray() });
        }

        return Results.Ok(result.Value);
    }
}
