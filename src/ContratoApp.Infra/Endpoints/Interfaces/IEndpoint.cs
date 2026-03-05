using Microsoft.AspNetCore.Routing;

namespace ContratoApp.Infra.Endpoints.Interfaces;

public interface IEndpoint
{
	void MapEndpoint(IEndpointRouteBuilder app);
}
