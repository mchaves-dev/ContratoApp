using ContratoApp.Dominio.Handlers;
using ContratoApp.Funcionalidades.OrdemServicoItem.Compartilhado.Response;
using ContratoApp.Infra.Database;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ContratoApp.Funcionalidades.OrdemServicoItem.ObterPorOrdemServico;

public interface IObterItensPorOrdemServicoHandler : IHandler
{
    Task<Result<IReadOnlyCollection<OrdemServicoItemResponse>>> HandleAsync(Guid idOrdemServico, CancellationToken cancellationToken);
}

public sealed class ObterItensPorOrdemServicoHandler(AppDbContext dbContext) : IObterItensPorOrdemServicoHandler
{
    public async Task<Result<IReadOnlyCollection<OrdemServicoItemResponse>>> HandleAsync(Guid idOrdemServico, CancellationToken cancellationToken)
    {
        var itens = await dbContext.OrdemServicoItens
            .AsNoTracking()
            .Where(x => x.IdOrdemServico == idOrdemServico)
            .OrderBy(x => x.Tipo)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        var response = itens
            .Select(item => new OrdemServicoItemResponse(
                item.IdOrdemServico,
                item.Descricao.Valor,
                item.Quantidade.Valor,
                item.ValorUnitario.Valor,
                item.Tipo))
            .ToList();

        return Result.Ok<IReadOnlyCollection<OrdemServicoItemResponse>>(response);
    }
}
