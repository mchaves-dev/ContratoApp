using ContratoApp.Dominio.Handlers;
using ContratoApp.Funcionalidades.Clientes.Compartilhado.Response;
using ContratoApp.Infra.Database;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ContratoApp.Funcionalidades.Clientes.ObterAtivos;

public interface IObterClientesAtivosHandler : IHandler
{
    Task<Result<IReadOnlyCollection<ClienteResponse>>> HandleAsync(CancellationToken cancellationToken);
}

public sealed class ObterClientesAtivosHandler(AppDbContext dbContext) : IObterClientesAtivosHandler
{
    public async Task<Result<IReadOnlyCollection<ClienteResponse>>> HandleAsync(CancellationToken cancellationToken)
    {
        var clientes = await dbContext.Clientes
            .AsNoTracking()
            .WhereAtivos()
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var response = clientes
            .OrderBy(x => x.Nome.Nome)
            .ThenBy(x => x.Nome.Sobrenome)
            .Select(cliente => cliente.ToResponse())
            .ToList();

        return Result.Ok<IReadOnlyCollection<ClienteResponse>>(response);
    }
}
