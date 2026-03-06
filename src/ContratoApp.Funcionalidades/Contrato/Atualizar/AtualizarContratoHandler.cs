using ContratoApp.Dominio.Events;
using ContratoApp.Dominio.Enums;
using ContratoApp.Dominio.Handlers;
using ContratoApp.Dominio.ValueObjects;
using ContratoApp.Funcionalidades.Clientes;
using ContratoApp.Funcionalidades.Compartilhado.Status;
using ContratoApp.Funcionalidades.Compartilhado.Validacoes;
using ContratoApp.Funcionalidades.Contrato.Compartilhado.Response;
using ContratoApp.Infra.Database;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ContratoApp.Funcionalidades.Contrato.Atualizar;

public sealed record AtualizarContratoRequest(
    Guid IdCliente,
    ETipoContrato Tipo,
    decimal Valor,
    DateTime DataInicio,
    DateTime? DataFim,
    EStatusContrato Status,
    string Observacoes);

public interface IAtualizarContratoHandler : IHandler
{
    Task<Result<ContratoResponse>> HandleAsync(Guid id, AtualizarContratoRequest request, CancellationToken cancellationToken);
}

public sealed class AtualizarContratoHandler(AppDbContext dbContext, IEventPublisher eventPublisher) : IAtualizarContratoHandler
{
    public async Task<Result<ContratoResponse>> HandleAsync(Guid id, AtualizarContratoRequest request, CancellationToken cancellationToken)
    {
        var contract = ContratoValidacoes.Validar(request.IdCliente, request.Valor, request.DataInicio, request.DataFim);
        if (!contract.IsValid)
            return contract.ToFailedResult<ContratoResponse>();

        var contrato = await dbContext.Contratos.FirstOrDefaultAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);
        if (contrato is null)
            return Result.Fail("Contrato não encontrado.");

        var clienteExiste = await dbContext.Clientes
            .AsNoTracking()
            .ExistePorIdAsync(request.IdCliente, cancellationToken)
            .ConfigureAwait(false);

        if (!clienteExiste)
            return Result.Fail("Cliente não encontrado.");

        contrato.IdCliente = request.IdCliente;
        contrato.Tipo = request.Tipo;
        contrato.Valor = Dinheiro.Criar(request.Valor);
        contrato.DataInicio = request.DataInicio;
        contrato.DataFim = request.DataFim;
        contrato.Status = StatusClassifier.ClassificarContrato(request.DataInicio, request.DataFim, request.Status);
        contrato.Observacoes = Observacao.Criar(request.Observacoes ?? string.Empty);

        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        await eventPublisher.PublishAsync(new ContratoAtualizadoEvent(contrato.Id, contrato.IdCliente), cancellationToken).ConfigureAwait(false);

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


