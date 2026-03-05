using ContratoApp.Funcionalidades.Clientes.Compartilhado.Rotas;
using ContratoApp.Infra.Endpoints.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ContratoApp.Funcionalidades.Clientes.Criar;

public sealed class CriarClienteRequest();

public class CriarClienteEndpoint : IEndpoint
{
	public void MapEndpoint(IEndpointRouteBuilder app)
	{
		app.MapPost(RotaConsts.Criar, HandleAsync)
			.WithTags("Clientes")
			.WithName("CriarCliente")
			.WithSummary("Cria um novo cliente.")
			.WithDescription("Cria um novo cliente com as informações fornecidas.")
			.Produces(StatusCodes.Status201Created)
			.Produces(StatusCodes.Status400BadRequest);
	}

	public Task HandleAsync(CriarClienteRequest request, ICriarClienteHandler handler, CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}
}