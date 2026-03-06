using ContratoApp.Dominio.Events;
using ContratoApp.Dominio.Enums;
using ContratoApp.Dominio.Handlers;
using ContratoApp.Dominio.ValueObjects;
using ContratoApp.Funcionalidades.Clientes;
using ContratoApp.Funcionalidades.Compartilhado.Validacoes;
using ContratoApp.Funcionalidades.Contrato.Compartilhado.Response;
using ContratoApp.Infra.Database;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ContratoApp.Funcionalidades.Contrato.Criar;

public sealed record CriarContratoRequest(
    Guid IdCliente,
    ETipoContrato Tipo,
    decimal Valor,
    DateTime DataInicio,
    DateTime? DataFim,
    EStatusContrato Status,
    string Observacoes);

public interface ICriarContratoHandler : IHandler
{
    Task<Result<ContratoResponse>> HandleAsync(CriarContratoRequest request, CancellationToken cancellationToken);
}

public sealed class CriarContratoHandler(AppDbContext dbContext, IEventPublisher eventPublisher) : ICriarContratoHandler
{
    public async Task<Result<ContratoResponse>> HandleAsync(CriarContratoRequest request, CancellationToken cancellationToken)
    {
        var contract = ContratoValidacoes.Validar(request.IdCliente, request.Valor, request.DataInicio, request.DataFim);
        if (!contract.IsValid)
            return contract.ToFailedResult<ContratoResponse>();

        var clienteExiste = await dbContext.Clientes
            .AsNoTracking()
            .ExistePorIdAsync(request.IdCliente, cancellationToken)
            .ConfigureAwait(false);

        if (!clienteExiste)
            return Result.Fail("Cliente não encontrado.");

        var contrato = new Dominio.Entities.Contrato
        {
            IdCliente = request.IdCliente,
            Tipo = request.Tipo,
            Valor = Dinheiro.Criar(request.Valor),
            DataInicio = request.DataInicio,
            DataFim = request.DataFim,
            Status = request.Status,
            Observacoes = Observacao.Criar(request.Observacoes ?? string.Empty)
        };

        await dbContext.Contratos.AddAsync(contrato, cancellationToken).ConfigureAwait(false);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        await eventPublisher.PublishAsync(new ContratoCriadoEvent(contrato.Id, contrato.IdCliente), cancellationToken).ConfigureAwait(false);

        return Result.Ok(ToResponse(contrato));
    }

    private static ContratoResponse ToResponse(Dominio.Entities.Contrato contrato)
    {
        return new ContratoResponse(
            contrato.Id,
            contrato.IdCliente,
            contrato.Tipo,
            contrato.Valor.Valor,
            contrato.DataInicio,
            contrato.DataFim,
            contrato.Status,
            contrato.Observacoes.Valor,
            contrato.CriadoEmUtc,
            contrato.AtualizadaEmUtc);
    }
}
