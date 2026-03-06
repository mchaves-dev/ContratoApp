using ContratoApp.Dominio.Entities;
using ContratoApp.Dominio.Enums;
using ContratoApp.Dominio.Events;
using ContratoApp.Dominio.Handlers;
using ContratoApp.Dominio.ValueObjects;
using ContratoApp.Infra.Database;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ContratoApp.Funcionalidades.OrdemServicoItem.RemoverItem;

public interface IRemoverOrdemServicoItemHandler : IHandler
{
    Task<Result> HandleAsync(Guid idOrdemServico, ETipoOrdemServicoItem tipo, string descricao, CancellationToken cancellationToken);
}

public sealed class RemoverOrdemServicoItemHandler(AppDbContext dbContext, IEventPublisher eventPublisher) : IRemoverOrdemServicoItemHandler
{
    public async Task<Result> HandleAsync(Guid idOrdemServico, ETipoOrdemServicoItem tipo, string descricao, CancellationToken cancellationToken)
    {
        var descricaoNormalizada = descricao?.Trim() ?? string.Empty;

        var item = await dbContext.OrdemServicoItens
            .FirstOrDefaultAsync(
                x => x.IdOrdemServico == idOrdemServico
                     && x.Tipo == tipo
                     && EF.Property<Descricao>(x, nameof(ContratoApp.Dominio.Entities.OrdemServicoItem.Descricao)).Equals(Descricao.Criar(descricaoNormalizada)),
                cancellationToken)
            .ConfigureAwait(false);

        if (item is null)
            return Result.Fail("Item não encontrado.");

        dbContext.OrdemServicoItens.Remove(item);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        await eventPublisher.PublishAsync(new OrdemServicoItemRemovidoEvent(item.IdOrdemServico, item.Descricao.Valor), cancellationToken).ConfigureAwait(false);

        return Result.Ok();
    }
}
