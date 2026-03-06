using ContratoApp.Dominio.Entities;
using ContratoApp.Dominio.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContratoApp.Infra.Database.Configurations;

public sealed class ContratoConfiguration : IEntityTypeConfiguration<Contrato>
{
    public void Configure(EntityTypeBuilder<Contrato> builder)
    {
        builder.ToTable("Contratos");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.IdCliente).IsRequired();

        builder.HasOne<Cliente>()
            .WithMany()
            .HasForeignKey(x => x.IdCliente)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.Tipo)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Valor)
            .HasConversion(
                valueObject => valueObject.Valor,
                value => Dinheiro.Criar(value))
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(x => x.DataInicio).IsRequired();
        builder.Property(x => x.DataFim).IsRequired(false);

        builder.Property(x => x.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Observacoes)
            .HasConversion(
                valueObject => valueObject.Valor,
                value => Observacao.Criar(value))
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(x => x.CriadoEmUtc).IsRequired();
        builder.Property(x => x.AtualizadaEmUtc).IsRequired(false);
    }
}
