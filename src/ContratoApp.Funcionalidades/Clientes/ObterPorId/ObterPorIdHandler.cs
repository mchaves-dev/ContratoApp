using ContratoApp.Dominio.Handlers;
using ContratoApp.Funcionalidades.Clientes.Compartilhado.Response;
using ContratoApp.Infra.Database;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ContratoApp.Funcionalidades.Clientes.ObterPorId;

public interface IObterClientePorIdHandler : IHandler
{
    Task<Result<ClienteResponse>> HandleAsync(Guid id, CancellationToken cancellationToken);
}

public sealed class ObterClientePorIdHandler(AppDbContext dbContext) : IObterClientePorIdHandler
{
    public async Task<Result<ClienteResponse>> HandleAsync(Guid id, CancellationToken cancellationToken)
    {
        var cliente = await dbContext.Clientes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            .ConfigureAwait(false);

        if (cliente is null)
            return Result.Fail("Cliente não encontrado.");

        return Result.Ok(cliente.ToResponse());
    }
}
