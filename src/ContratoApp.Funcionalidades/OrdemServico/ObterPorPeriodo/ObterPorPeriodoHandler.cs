using ContratoApp.Dominio.Handlers;
using ContratoApp.Funcionalidades.Compartilhado.Validacoes;
using ContratoApp.Funcionalidades.OrdemServico.Compartilhado.Response;
using ContratoApp.Infra.Database;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ContratoApp.Funcionalidades.OrdemServico.ObterPorPeriodo;

public interface IObterOrdensServicoPorPeriodoHandler : IHandler
{
    Task<Result<IReadOnlyCollection<OrdemServicoResponse>>> HandleAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken);
}

public sealed class ObterOrdensServicoPorPeriodoHandler(AppDbContext dbContext) : IObterOrdensServicoPorPeriodoHandler
{
    public async Task<Result<IReadOnlyCollection<OrdemServicoResponse>>> HandleAsync(DateTime inicio, DateTime fim, CancellationToken cancellationToken)
    {
        var contract = PeriodoValidacoes.Validar(inicio, fim);
        if (!contract.IsValid)
            return contract.ToFailedResult<IReadOnlyCollection<OrdemServicoResponse>>();

        var ordens = await dbContext.OrdemServicos
            .AsNoTracking()
            .Where(x => x.DataAbertura >= inicio && x.DataAbertura <= fim)
            .OrderBy(x => x.DataAbertura)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var response = ordens
            .Select(ordem => new OrdemServicoResponse(
                ordem.Id,
                ordem.IdCliente,
                ordem.DataAbertura,
                ordem.DataFechamento,
                ordem.Status,
                ordem.Observacoes.Valor,
                ordem.CriadoEmUtc,
                ordem.AtualizadaEmUtc))
            .ToList();

        return Result.Ok<IReadOnlyCollection<OrdemServicoResponse>>(response);
    }
}
