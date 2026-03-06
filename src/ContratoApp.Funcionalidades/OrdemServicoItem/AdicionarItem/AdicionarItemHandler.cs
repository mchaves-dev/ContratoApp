using ContratoApp.Dominio.Events;
using ContratoApp.Dominio.Enums;
using ContratoApp.Dominio.Handlers;
using ContratoApp.Dominio.ValueObjects;
using ContratoApp.Funcionalidades.Compartilhado.Validacoes;
using ContratoApp.Funcionalidades.OrdemServicoItem.Compartilhado.Response;
using ContratoApp.Infra.Database;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ContratoApp.Funcionalidades.OrdemServicoItem.AdicionarItem;

public sealed record AdicionarOrdemServicoItemRequest(
    Guid IdOrdemServico,
    string Descricao,
    int Quantidade,
    decimal ValorUnitario,
    ETipoOrdemServicoItem Tipo);

public interface IAdicionarOrdemServicoItemHandler : IHandler
{
    Task<Result<OrdemServicoItemResponse>> HandleAsync(AdicionarOrdemServicoItemRequest request, CancellationToken cancellationToken);
}

public sealed class AdicionarOrdemServicoItemHandler(AppDbContext dbContext, IEventPublisher eventPublisher) : IAdicionarOrdemServicoItemHandler
{
    public async Task<Result<OrdemServicoItemResponse>> HandleAsync(AdicionarOrdemServicoItemRequest request, CancellationToken cancellationToken)
    {
        var descricaoNormalizada = request.Descricao?.Trim() ?? string.Empty;

        var contract = OrdemServicoItemValidacoes.Validar(request.IdOrdemServico, descricaoNormalizada, request.Quantidade, request.ValorUnitario);
        if (!contract.IsValid)
            return contract.ToFailedResult<OrdemServicoItemResponse>();

        var ordemExiste = await dbContext.OrdemServicos
            .AsNoTracking()
            .AnyAsync(x => x.Id == request.IdOrdemServico, cancellationToken)
            .ConfigureAwait(false);

        if (!ordemExiste)
            return Result.Fail("Ordem de serviço não encontrada.");

        var itemExiste = await dbContext.OrdemServicoItens
            .AsNoTracking()
            .AnyAsync(
                x => x.IdOrdemServico == request.IdOrdemServico
                     && x.Tipo == request.Tipo
                     && EF.Property<Descricao>(x, nameof(Dominio.Entities.OrdemServicoItem.Descricao)).Equals(Descricao.Criar(descricaoNormalizada)),
                cancellationToken)
            .ConfigureAwait(false);

        if (itemExiste)
            return Result.Fail("Já existe item com a mesma chave para esta ordem de serviço.");

        var item = new Dominio.Entities.OrdemServicoItem
        {
            IdOrdemServico = request.IdOrdemServico,
            Descricao = Descricao.Criar(descricaoNormalizada),
            Quantidade = Quantidade.Criar(request.Quantidade),
            ValorUnitario = Dinheiro.Criar(request.ValorUnitario),
            Tipo = request.Tipo
        };

        await dbContext.OrdemServicoItens.AddAsync(item, cancellationToken).ConfigureAwait(false);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        await eventPublisher.PublishAsync(new OrdemServicoItemAdicionadoEvent(item.IdOrdemServico, item.Descricao.Valor), cancellationToken).ConfigureAwait(false);

        return Result.Ok(new OrdemServicoItemResponse(
            item.IdOrdemServico,
            item.Descricao.Valor,
            item.Quantidade.Valor,
            item.ValorUnitario.Valor,
            item.Tipo));
    }
}
