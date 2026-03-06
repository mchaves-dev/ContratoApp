using ContratoApp.Funcionalidades.OrdemServico.Compartilhado.Response;
using ContratoApp.Funcionalidades.OrdemServico.Compartilhado.Rotas;
using ContratoApp.Infra.Endpoints.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ContratoApp.Funcionalidades.OrdemServico.ObterAtivos;

public sealed class ObterOrdensServicoAtivasEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(RotaConsts.ObterAtivos, HandleAsync)
            .WithTags("Ordens de Serviço")
            .WithName("ObterOrdensServicoAtivas")
            .WithSummary("Lista ordens de serviço ativas.")
            .Produces<IReadOnlyCollection<OrdemServicoResponse>>(StatusCodes.Status200OK);
    }

    public static async Task<IResult> HandleAsync(IObterOrdensServicoAtivasHandler handler, CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(cancellationToken).ConfigureAwait(false);
        return Results.Ok(result.Value);
    }
}
