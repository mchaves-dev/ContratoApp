using Microsoft.AspNetCore.Routing;

namespace ContratoApp.Infra.Endpoints.Interfaces;

internal interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
