using ContratoApp.Funcionalidades.Clientes.Compartilhado.Response;
using ContratoApp.Funcionalidades.Clientes.Compartilhado.Rotas;
using ContratoApp.Infra.Endpoints.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ContratoApp.Funcionalidades.Clientes.ObterAtivos;

public sealed class ObterClientesAtivosEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(RotaConsts.ObterAtivos, HandleAsync)
            .WithTags("Clientes")
            .WithName("ObterClientesAtivos")
            .WithSummary("Lista clientes ativos.")
            .Produces<IReadOnlyCollection<ClienteResponse>>(StatusCodes.Status200OK);
    }

    public static async Task<IResult> HandleAsync(IObterClientesAtivosHandler handler, CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(cancellationToken).ConfigureAwait(false);
        return Results.Ok(result.Value);
    }
}
