using ContratoApp.Dominio.Enums;
using ContratoApp.Dominio.Handlers;
using ContratoApp.Funcionalidades.OrdemServico.Compartilhado.Response;
using ContratoApp.Infra.Database;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ContratoApp.Funcionalidades.OrdemServico.ObterAtivos;

public interface IObterOrdensServicoAtivasHandler : IHandler
{
    Task<Result<IReadOnlyCollection<OrdemServicoResponse>>> HandleAsync(CancellationToken cancellationToken);
}

public sealed class ObterOrdensServicoAtivasHandler(AppDbContext dbContext) : IObterOrdensServicoAtivasHandler
{
    public async Task<Result<IReadOnlyCollection<OrdemServicoResponse>>> HandleAsync(CancellationToken cancellationToken)
    {
        var ordens = await dbContext.OrdemServicos
            .AsNoTracking()
            .Where(x => x.Status == EStatusOrdemServico.Aberta || x.Status == EStatusOrdemServico.EmAndamento)
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
