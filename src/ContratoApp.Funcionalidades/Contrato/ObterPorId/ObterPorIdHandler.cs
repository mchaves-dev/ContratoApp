using ContratoApp.Dominio.Handlers;
using ContratoApp.Funcionalidades.Contrato.Compartilhado.Response;
using ContratoApp.Infra.Database;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ContratoApp.Funcionalidades.Contrato.ObterPorId;

public interface IObterContratoPorIdHandler : IHandler
{
    Task<Result<ContratoResponse>> HandleAsync(Guid id, CancellationToken cancellationToken);
}

public sealed class ObterContratoPorIdHandler(AppDbContext dbContext) : IObterContratoPorIdHandler
{
    public async Task<Result<ContratoResponse>> HandleAsync(Guid id, CancellationToken cancellationToken)
    {
        var contrato = await dbContext.Contratos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            .ConfigureAwait(false);

        if (contrato is null)
            return Result.Fail("Contrato não encontrado.");

        return Result.Ok(new ContratoResponse(
            contrato.Id,
            contrato.IdCliente,
            contrato.Tipo,
            contrato.Valor.Valor,
            contrato.DataInicio,
            contrato.DataFim,
            contrato.Status,
            contrato.Observacoes.Valor,
            contrato.CriadoEmUtc,
            contrato.AtualizadaEmUtc));
    }
}
