using ContratoApp.Dominio.Entities;
using ContratoApp.Dominio.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContratoApp.Infra.Database.Configurations;

public sealed class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("Clientes");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.Ativo)
            .HasConversion(
                valueObject => valueObject.Value,
                value => value ? Ativo.Ativar() : Ativo.Inativar())
            .IsRequired();

        builder.Property(x => x.Nome)
            .HasConversion(
                valueObject => $"{valueObject.Nome}|{valueObject.Sobrenome}",
                value => ParseNomePessoa(value))
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasConversion(
                valueObject => valueObject.Endereco,
                value => Email.Criar(value))
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Endereco)
            .HasConversion(
                valueObject => $"{valueObject.Cidade}|{valueObject.Logradouro}|{valueObject.Bairro}|{valueObject.Numero}|{valueObject.Cep}",
                value => ParseEndereco(value))
            .HasMaxLength(400)
            .IsRequired();

        builder.Property(x => x.CriadoEmUtc).IsRequired();
        builder.Property(x => x.AtualizadaEmUtc).IsRequired(false);
    }

    private static NomePessoa ParseNomePessoa(string value)
    {
        var partes = value.Split('|', 2, StringSplitOptions.TrimEntries);
        var nome = partes.Length > 0 ? partes[0] : string.Empty;
        var sobrenome = partes.Length > 1 ? partes[1] : string.Empty;
        return NomePessoa.Criar(nome, sobrenome);
    }

    private static Endereco ParseEndereco(string value)
    {
        var partes = value.Split('|', 5, StringSplitOptions.TrimEntries);
        var cidade = partes.Length > 0 ? partes[0] : string.Empty;
        var logradouro = partes.Length > 1 ? partes[1] : string.Empty;
        var bairro = partes.Length > 2 ? partes[2] : string.Empty;
        var numero = partes.Length > 3 ? partes[3] : string.Empty;
        var cep = partes.Length > 4 ? partes[4] : string.Empty;
        return Endereco.Criar(cidade, logradouro, bairro, numero, cep);
    }
}
