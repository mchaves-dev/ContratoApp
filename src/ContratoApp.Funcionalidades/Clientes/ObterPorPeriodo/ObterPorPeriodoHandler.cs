using ContratoApp.Dominio.Handlers;
using ContratoApp.Funcionalidades.Clientes.Compartilhado.Response;
using ContratoApp.Funcionalidades.Compartilhado.Validacoes;
using ContratoApp.Infra.Database;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ContratoApp.Funcionalidades.Clientes.ObterPorPeriodo;

public interface IObterClientesPorPeriodoHandler : IHandler
{
    Task<Result<IReadOnlyCollection<ClienteResponse>>> HandleAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken);
}

public sealed class ObterClientesPorPeriodoHandler(AppDbContext dbContext) : IObterClientesPorPeriodoHandler
{
    public async Task<Result<IReadOnlyCollection<ClienteResponse>>> HandleAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken)
    {
        var contract = PeriodoValidacoes.Validar(inicio, fim);
        if (!contract.IsValid)
            return contract.ToFailedResult<IReadOnlyCollection<ClienteResponse>>();

        var clientes = await dbContext.Clientes
            .AsNoTracking()
            .Where(x => x.CriadoEmUtc >= inicio && x.CriadoEmUtc <= fim)
            .OrderBy(x => x.CriadoEmUtc)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return Result.Ok<IReadOnlyCollection<ClienteResponse>>(clientes.Select(x => x.ToResponse()).ToList());
    }
}
