using ContratoApp.Dominio.Handlers;
using ContratoApp.Funcionalidades.Clientes.Compartilhado.Response;
using FluentResults;

namespace ContratoApp.Funcionalidades.Clientes.Criar;

internal interface ICriarClienteHandler : IHandler
{
	Task<Result<ClienteResponse>> HandleAsync(CriarClienteRequest request, CancellationToken cancellationToken);
}

internal class CriarClienteHandler : ICriarClienteHandler
{
	public Task<Result<ClienteResponse>> HandleAsync(CriarClienteRequest request, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
}