using ContratoApp.Dominio.Entities;
using ContratoApp.Dominio.Events;
using ContratoApp.Dominio.Handlers;
using ContratoApp.Dominio.ValueObjects;
using ContratoApp.Funcionalidades.Clientes.Compartilhado.Response;
using ContratoApp.Funcionalidades.Compartilhado.Validacoes;
using ContratoApp.Infra.Database;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ContratoApp.Funcionalidades.Clientes.Criar;

public sealed record CriarClienteRequest(
    string Nome,
    string Sobrenome,
    string Email,
    string Cidade,
    string Logradouro,
    string Bairro,
    string Numero,
    string Cep);

public interface ICriarClienteHandler : IHandler
{
    Task<Result<ClienteResponse>> HandleAsync(CriarClienteRequest request, CancellationToken cancellationToken);
}

public sealed class CriarClienteHandler(AppDbContext dbContext, IEventPublisher eventPublisher) : ICriarClienteHandler
{
    public async Task<Result<ClienteResponse>> HandleAsync(CriarClienteRequest request, CancellationToken cancellationToken)
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

        var emailNormalizado = request.Email.Trim();
        var existeEmail = await dbContext.Clientes
            .AsNoTracking()
            .ExistePorEmailAsync(emailNormalizado, cancellationToken)
            .ConfigureAwait(false);

        if (existeEmail)
            return Result.Fail("Já existe cliente com este email.");

        var cliente = new Cliente
        {
            Ativo = Ativo.Criar(),
            Nome = NomePessoa.Criar(request.Nome, request.Sobrenome),
            Email = Email.Criar(request.Email),
            Endereco = Endereco.Criar(request.Cidade, request.Logradouro, request.Bairro, request.Numero, request.Cep)
        };

        await dbContext.Clientes.AddAsync(cliente, cancellationToken).ConfigureAwait(false);
        await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        await eventPublisher.PublishAsync(new ClienteCriadoEvent(cliente.Id), cancellationToken).ConfigureAwait(false);

        return Result.Ok(cliente.ToResponse());
    }
}
