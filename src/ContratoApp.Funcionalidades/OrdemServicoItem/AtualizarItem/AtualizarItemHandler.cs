using ContratoApp.Dominio.Events;
using ContratoApp.Dominio.Enums;
using ContratoApp.Dominio.Handlers;
using ContratoApp.Dominio.ValueObjects;
using ContratoApp.Funcionalidades.Compartilhado.Validacoes;
using ContratoApp.Funcionalidades.OrdemServicoItem.Compartilhado.Response;
using ContratoApp.Infra.Database;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ContratoApp.Funcionalidades.OrdemServicoItem.AtualizarItem;

public sealed record AtualizarOrdemServicoItemRequest(
    Guid IdOrdemServico,
    string Descricao,
    ETipoOrdemServicoItem Tipo,
    int Quantidade,
    decimal ValorUnitario);

public interface IAtualizarOrdemServicoItemHandler : IHandler
{
    Task<Result<OrdemServicoItemResponse>> HandleAsync(AtualizarOrdemServicoItemRequest request, CancellationToken cancellationToken);
}

public sealed class AtualizarOrdemServicoItemHandler(AppDbContext dbContext, IEventPublisher eventPublisher) : IAtualizarOrdemServicoItemHandler
{
    public async Task<Result<OrdemServicoItemResponse>> HandleAsync(AtualizarOrdemServicoItemRequest request, CancellationToken cancellationToken)
    {
        var descricaoNormalizada = request.Descricao?.Trim() ?? string.Empty;

        var contract = OrdemServicoItemValidacoes.Validar(request.IdOrdemServico, descricaoNormalizada, request.Quantidade, request.ValorUnitario);
        if (!contract.IsValid)
            return contract.ToFailedResult<OrdemServicoItemResponse>();

        var item = await dbContext.OrdemServicoItens
            .FirstOrDefaultAsync(
                x => x.IdOrdemServico == request.IdOrdemServico
                     && x.Tipo == request.Tipo
                     && EF.Property<Descricao>(x, nameof(ContratoApp.Dominio.Entities.OrdemServicoItem.Descricao)).Equals(Descricao.Criar(descricaoNormalizada)),
                cancellationToken)
            .ConfigureAwait(false);

        if (item is null)
            return Result.Fail("Item não encontrado.");

        item.Quantidade = Quantidade.Criar(request.Quantidade);
        item.ValorUnitario = Dinheiro.Criar(request.ValorUnitario);

        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        await eventPublisher.PublishAsync(new OrdemServicoItemAtualizadoEvent(item.IdOrdemServico, item.Descricao.Valor), cancellationToken).ConfigureAwait(false);

        return Result.Ok(new OrdemServicoItemResponse(
            item.IdOrdemServico,
            item.Descricao.Valor,
            item.Quantidade.Valor,
            item.ValorUnitario.Valor,
            item.Tipo));
    }
}
