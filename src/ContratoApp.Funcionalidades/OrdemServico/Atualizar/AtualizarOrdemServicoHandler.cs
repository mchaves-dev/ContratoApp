using ContratoApp.Dominio.Enums;
using ContratoApp.Dominio.Events;
using ContratoApp.Dominio.Handlers;
using ContratoApp.Dominio.ValueObjects;
using ContratoApp.Funcionalidades.Clientes;
using ContratoApp.Funcionalidades.Compartilhado.Status;
using ContratoApp.Funcionalidades.Compartilhado.Validacoes;
using ContratoApp.Funcionalidades.OrdemServico.Compartilhado.Response;
using ContratoApp.Infra.Database;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ContratoApp.Funcionalidades.OrdemServico.Atualizar;

public sealed record AtualizarOrdemServicoRequest(
    Guid IdCliente,
    DateTime DataAbertura,
    DateTime? DataFechamento,
    EStatusOrdemServico Status,
    string Observacoes);

public interface IAtualizarOrdemServicoHandler : IHandler
{
    Task<Result<OrdemServicoResponse>> HandleAsync(Guid id, AtualizarOrdemServicoRequest request, CancellationToken cancellationToken);
}

public sealed class AtualizarOrdemServicoHandler(AppDbContext dbContext, IEventPublisher eventPublisher) : IAtualizarOrdemServicoHandler
{
    public async Task<Result<OrdemServicoResponse>> HandleAsync(Guid id, AtualizarOrdemServicoRequest request, CancellationToken cancellationToken)
    {
        var contract = OrdemServicoValidacoes.Validar(request.IdCliente, request.DataAbertura, request.DataFechamento);
        if (!contract.IsValid)
            return contract.ToFailedResult<OrdemServicoResponse>();

        var ordemServico = await dbContext.OrdemServicos.FirstOrDefaultAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);
        if (ordemServico is null)
            return Result.Fail("Ordem de serviço não encontrada.");

        var clienteExiste = await dbContext.Clientes
            .AsNoTracking()
            .ExistePorIdAsync(request.IdCliente, cancellationToken)
            .ConfigureAwait(false);

        if (!clienteExiste)
            return Result.Fail("Cliente não encontrado.");

        ordemServico.IdCliente = request.IdCliente;
        ordemServico.DataAbertura = request.DataAbertura;
        ordemServico.DataFechamento = request.DataFechamento;
        ordemServico.Status = StatusClassifier.ClassificarOrdemServico(request.DataAbertura, request.DataFechamento, request.Status);
        ordemServico.Observacoes = Observacao.Criar(request.Observacoes ?? string.Empty);

        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        await eventPublisher.PublishAsync(new OrdemServicoAtualizadaEvent(ordemServico.Id, ordemServico.IdCliente), cancellationToken).ConfigureAwait(false);

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


