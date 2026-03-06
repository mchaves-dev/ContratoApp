using ContratoApp.Dominio.Handlers;
using ContratoApp.Funcionalidades.Contrato.Compartilhado.Response;
using ContratoApp.Infra.Database;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ContratoApp.Funcionalidades.Contrato.ObterAtivos;

public interface IObterContratosAtivosHandler : IHandler
{
    Task<Result<IReadOnlyCollection<ContratoResponse>>> HandleAsync(CancellationToken cancellationToken);
}

public sealed class ObterContratosAtivosHandler(AppDbContext dbContext) : IObterContratosAtivosHandler
{
    public async Task<Result<IReadOnlyCollection<ContratoResponse>>> HandleAsync(CancellationToken cancellationToken)
    {
        var hoje = DateTime.UtcNow.Date;

        var contratos = await dbContext.Contratos
            .AsNoTracking()
            .Where(x => !x.DataFim.HasValue || x.DataFim.Value >= hoje)
            .OrderBy(x => x.DataInicio)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var response = contratos
            .Select(contrato => new ContratoResponse(
                contrato.Id,
                contrato.IdCliente,
                contrato.Tipo,
                contrato.Valor.Valor,
                contrato.DataInicio,
                contrato.DataFim,
                contrato.Status,
                contrato.Observacoes.Valor,
                contrato.CriadoEmUtc,
                contrato.AtualizadaEmUtc))
            .ToList();

        return Result.Ok<IReadOnlyCollection<ContratoResponse>>(response);
    }
}
