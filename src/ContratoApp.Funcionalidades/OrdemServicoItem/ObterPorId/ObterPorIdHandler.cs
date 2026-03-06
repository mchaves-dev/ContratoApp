using ContratoApp.Dominio.Entities;
using ContratoApp.Dominio.Enums;
using ContratoApp.Dominio.Handlers;
using ContratoApp.Dominio.ValueObjects;
using ContratoApp.Funcionalidades.OrdemServicoItem.Compartilhado.Response;
using ContratoApp.Infra.Database;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ContratoApp.Funcionalidades.OrdemServicoItem.ObterPorId;

public interface IObterOrdemServicoItemPorIdHandler : IHandler
{
    Task<Result<OrdemServicoItemResponse>> HandleAsync(Guid idOrdemServico, ETipoOrdemServicoItem tipo, string descricao, CancellationToken cancellationToken);
}

public sealed class ObterOrdemServicoItemPorIdHandler(AppDbContext dbContext) : IObterOrdemServicoItemPorIdHandler
{
    public async Task<Result<OrdemServicoItemResponse>> HandleAsync(Guid idOrdemServico, ETipoOrdemServicoItem tipo, string descricao, CancellationToken cancellationToken)
    {
        var descricaoNormalizada = descricao?.Trim() ?? string.Empty;

        var item = await dbContext.OrdemServicoItens
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.IdOrdemServico == idOrdemServico
                     && x.Tipo == tipo
                     && EF.Property<Descricao>(x, nameof(ContratoApp.Dominio.Entities.OrdemServicoItem.Descricao)).Equals(Descricao.Criar(descricaoNormalizada)),
                cancellationToken)
            .ConfigureAwait(false);

        if (item is null)
            return Result.Fail("Item não encontrado.");

        return Result.Ok(new OrdemServicoItemResponse(
            item.IdOrdemServico,
            item.Descricao.Valor,
            item.Quantidade.Valor,
            item.ValorUnitario.Valor,
            item.Tipo));
    }
}
