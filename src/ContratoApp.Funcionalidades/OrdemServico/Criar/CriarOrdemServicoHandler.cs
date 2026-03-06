using ContratoApp.Dominio.Events;
using ContratoApp.Dominio.Enums;
using ContratoApp.Dominio.Handlers;
using ContratoApp.Dominio.ValueObjects;
using ContratoApp.Funcionalidades.Clientes;
using ContratoApp.Funcionalidades.Compartilhado.Validacoes;
using ContratoApp.Funcionalidades.OrdemServico.Compartilhado.Response;
using ContratoApp.Infra.Database;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ContratoApp.Funcionalidades.OrdemServico.Criar;

public sealed record CriarOrdemServicoRequest(
    Guid IdCliente,
    DateTime DataAbertura,
    DateTime? DataFechamento,
    EStatusOrdemServico Status,
    string Observacoes);

public interface ICriarOrdemServicoHandler : IHandler
{
    Task<Result<OrdemServicoResponse>> HandleAsync(CriarOrdemServicoRequest request, CancellationToken cancellationToken);
}

public sealed class CriarOrdemServicoHandler(AppDbContext dbContext, IEventPublisher eventPublisher) : ICriarOrdemServicoHandler
{
    public async Task<Result<OrdemServicoResponse>> HandleAsync(CriarOrdemServicoRequest request, CancellationToken cancellationToken)
    {
        var contract = OrdemServicoValidacoes.Validar(request.IdCliente, request.DataAbertura, request.DataFechamento);
        if (!contract.IsValid)
            return contract.ToFailedResult<OrdemServicoResponse>();

        var clienteExiste = await dbContext.Clientes
            .AsNoTracking()
            .ExistePorIdAsync(request.IdCliente, cancellationToken)
            .ConfigureAwait(false);

        if (!clienteExiste)
            return Result.Fail("Cliente não encontrado.");

        var ordemServico = new Dominio.Entities.OrdemServico
        {
            IdCliente = request.IdCliente,
            DataAbertura = request.DataAbertura,
            DataFechamento = request.DataFechamento,
            Status = request.Status,
            Observacoes = Observacao.Criar(request.Observacoes ?? string.Empty)
        };

        await dbContext.OrdemServicos.AddAsync(ordemServico, cancellationToken).ConfigureAwait(false);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        await eventPublisher.PublishAsync(new OrdemServicoCriadaEvent(ordemServico.Id, ordemServico.IdCliente), cancellationToken).ConfigureAwait(false);

        return Result.Ok(new OrdemServicoResponse(
            ordemServico.Id,
            ordemServico.IdCliente,
            ordemServico.DataAbertura,
            ordemServico.DataFechamento,
            ordemServico.Status,
            ordemServico.Observacoes.Valor,
            ordemServico.CriadoEmUtc,
            ordemServico.AtualizadaEmUtc));
    }
}
