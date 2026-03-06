using ContratoApp.Funcionalidades.Contrato.Compartilhado.Response;
using ContratoApp.Funcionalidades.Contrato.Compartilhado.Rotas;
using ContratoApp.Infra.Endpoints.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ContratoApp.Funcionalidades.Contrato.ObterAtivos;

public sealed class ObterContratosAtivosEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(RotaConsts.ObterAtivos, HandleAsync)
            .WithTags("Contratos")
            .WithName("ObterContratosAtivos")
            .WithSummary("Lista contratos ativos.")
            .Produces<IReadOnlyCollection<ContratoResponse>>(StatusCodes.Status200OK);
    }

    public static async Task<IResult> HandleAsync(IObterContratosAtivosHandler handler, CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(cancellationToken).ConfigureAwait(false);
        return Results.Ok(result.Value);
    }
}
