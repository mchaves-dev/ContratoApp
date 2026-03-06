using ContratoApp.Dominio.Entities;
using ContratoApp.Dominio.ValueObjects;
using ContratoApp.Funcionalidades.Clientes.Compartilhado.Response;
using Microsoft.EntityFrameworkCore;

namespace ContratoApp.Funcionalidades.Clientes;

public static class ClienteQuery
{
    public static IQueryable<Cliente> WhereAtivos(this IQueryable<Cliente> query)
    {
        return query.Where(cliente => cliente.Ativo);
    }

    public static Task<bool> ExistePorIdAsync(this IQueryable<Cliente> query, Guid id, CancellationToken cancellationToken)
    {
        return query.AnyAsync(cliente => cliente.Id == id, cancellationToken);
    }

    public static Task<bool> ExistePorEmailAsync(this IQueryable<Cliente> query, string emailNormalizado, CancellationToken cancellationToken)
    {
        var email = Email.Criar(emailNormalizado);
        return query.AnyAsync(cliente => EF.Property<Email>(cliente, nameof(Cliente.Email)).Equals(email), cancellationToken);
    }

    public static Task<bool> ExisteOutroPorEmailAsync(this IQueryable<Cliente> query, Guid idAtual, string emailNormalizado, CancellationToken cancellationToken)
    {
        var email = Email.Criar(emailNormalizado);
        return query.AnyAsync(
            cliente => cliente.Id != idAtual && EF.Property<Email>(cliente, nameof(Cliente.Email)).Equals(email),
            cancellationToken);
    }

    public static ClienteResponse ToResponse(this Cliente cliente)
    {
        return new ClienteResponse(
            cliente.Id,
            cliente.Nome.Nome,
            cliente.Nome.Sobrenome,
            cliente.Email.Endereco,
            cliente.Endereco.Cidade,
            cliente.Endereco.Logradouro,
            cliente.Endereco.Bairro,
            cliente.Endereco.Numero,
            cliente.Endereco.Cep,
            cliente.Ativo.Value,
            cliente.CriadoEmUtc);
    }
}
