using ContratoApp.Dominio.Handlers;
using ContratoApp.Funcionalidades.Compartilhado.Validacoes;
using ContratoApp.Funcionalidades.Contrato.Compartilhado.Response;
using ContratoApp.Infra.Database;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ContratoApp.Funcionalidades.Contrato.ObterPorPeriodo;

public interface IObterContratosPorPeriodoHandler : IHandler
{
    Task<Result<IReadOnlyCollection<ContratoResponse>>> HandleAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken);
}

public sealed class ObterContratosPorPeriodoHandler(AppDbContext dbContext) : IObterContratosPorPeriodoHandler
{
    public async Task<Result<IReadOnlyCollection<ContratoResponse>>> HandleAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken)
    {
        var contract = PeriodoValidacoes.Validar(inicio, fim);
        if (!contract.IsValid)
            return contract.ToFailedResult<IReadOnlyCollection<ContratoResponse>>();

        var contratos = await dbContext.Contratos
            .AsNoTracking()
            .Where(x => x.DataInicio >= inicio && x.DataInicio <= fim)
            .OrderBy(x => x.DataInicio)
            .ToListAsync()
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
