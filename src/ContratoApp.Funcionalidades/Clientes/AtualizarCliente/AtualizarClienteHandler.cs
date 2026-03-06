using ContratoApp.Dominio.Entities;
using ContratoApp.Dominio.Events;
using ContratoApp.Dominio.Handlers;
using ContratoApp.Dominio.ValueObjects;
using ContratoApp.Funcionalidades.Clientes.Compartilhado.Response;
using ContratoApp.Funcionalidades.Compartilhado.Validacoes;
using ContratoApp.Infra.Database;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ContratoApp.Funcionalidades.Clientes.AtualizarCliente;

public sealed record AtualizarClienteRequest(
    string Nome,
    string Sobrenome,
    string Email,
    string Cidade,
    string Logradouro,
    string Bairro,
    string Numero,
    string Cep,
    bool Ativo);

public interface IAtualizarClienteHandler : IHandler
{
    Task<Result<ClienteResponse>> HandleAsync(Guid id, AtualizarClienteRequest request, CancellationToken cancellationToken);
}

public sealed class AtualizarClienteHandler(AppDbContext dbContext, IEventPublisher eventPublisher) : IAtualizarClienteHandler
{
    public async Task<Result<ClienteResponse>> HandleAsync(Guid id, AtualizarClienteRequest request, CancellationToken cancellationToken)
    {
        var contract = ClienteValidacoes.ValidarDados(
            request.Nome,
            request.Sobrenome,
            request.Email,
            request.Cidade,
            request.Logradouro,
            request.Bairro,
            request.Numero,
            request.Cep);

        if (!contract.IsValid)
            return contract.ToFailedResult<ClienteResponse>();

        var cliente = await dbContext.Clientes.FirstOrDefaultAsync(x => x.Id == id, cancellationToken).ConfigureAwait(false);
        if (cliente is null)
            return Result.Fail("Cliente não encontrado.");

        var emailNormalizado = request.Email.Trim();
        var emailDuplicado = await dbContext.Clientes
            .AsNoTracking()
            .ExisteOutroPorEmailAsync(id, emailNormalizado, cancellationToken)
            .ConfigureAwait(false);

        if (emailDuplicado)
            return Result.Fail("Já existe cliente com este email.");

        cliente.Nome = NomePessoa.Criar(request.Nome, request.Sobrenome);
        cliente.Email = Email.Criar(request.Email);
        cliente.Endereco = Endereco.Criar(request.Cidade, request.Logradouro, request.Bairro, request.Numero, request.Cep);
        cliente.Ativo = request.Ativo ? Ativo.Ativar() : Ativo.Inativar();

        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        await eventPublisher.PublishAsync(new ClienteAtualizadoEvent(cliente.Id), cancellationToken).ConfigureAwait(false);

        return Result.Ok(cliente.ToResponse());
    }
}
