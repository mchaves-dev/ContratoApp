using ContratoApp.Funcionalidades.Clientes.Compartilhado.Response;
using ContratoApp.Funcionalidades.Clientes.Compartilhado.Rotas;
using ContratoApp.Infra.Endpoints.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace ContratoApp.Funcionalidades.Clientes.ObterPorPeriodo;

public sealed class ObterClientesPorPeriodoEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(RotaConsts.ObterPorPeriodo, HandleAsync)
            .WithTags("Clientes")
            .WithName("ObterClientesPorPeriodo")
            .WithSummary("Lista clientes por período de criação (UTC).")
            .Produces<IReadOnlyCollection<ClienteResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
    }

    public static async Task<IResult> HandleAsync(
        [FromQuery] DateTime inicio,
        [FromQuery] DateTime fim,
        IObterClientesPorPeriodoHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(inicio, fim, cancellationToken).ConfigureAwait(false);

        if (result.IsFailed)
            return Results.BadRequest(new { erros = result.Errors.Select(x => x.Message).ToArray() });

        return Results.Ok(result.Value);
    }
}
