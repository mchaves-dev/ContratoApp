using ContratoApp.Funcionalidades.Contrato.Compartilhado.Response;
using ContratoApp.Funcionalidades.Contrato.Compartilhado.Rotas;
using ContratoApp.Infra.Endpoints.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace ContratoApp.Funcionalidades.Contrato.Criar;

public sealed class CriarContratoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(RotaConsts.Criar, HandleAsync)
            .WithTags("Contratos")
            .WithName("CriarContrato")
            .WithSummary("Cria um novo contrato.")
            .Produces<ContratoResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);
    }

    public static async Task<IResult> HandleAsync(
        [FromBody] CriarContratoRequest request,
        ICriarContratoHandler handler,
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
