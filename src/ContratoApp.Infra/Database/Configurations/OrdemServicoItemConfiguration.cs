using ContratoApp.Dominio.Entities;
using ContratoApp.Dominio.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContratoApp.Infra.Database.Configurations;

public sealed class OrdemServicoItemConfiguration : IEntityTypeConfiguration<OrdemServicoItem>
{
    public void Configure(EntityTypeBuilder<OrdemServicoItem> builder)
    {
        builder.ToTable("OrdemServicoItens");

        builder.HasKey(x => new { x.IdOrdemServico, x.Tipo, x.Descricao });

        builder.Property(x => x.IdOrdemServico).IsRequired();

        builder.HasOne<OrdemServico>()
            .WithMany()
            .HasForeignKey(x => x.IdOrdemServico)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Descricao)
            .HasConversion(
                valueObject => valueObject.Valor,
                value => Descricao.Criar(value))
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.Quantidade)
            .HasConversion(
                valueObject => valueObject.Valor,
                value => Quantidade.Criar(value))
            .IsRequired();

        builder.Property(x => x.ValorUnitario)
            .HasConversion(
                valueObject => valueObject.Valor,
                value => Dinheiro.Criar(value))
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(x => x.Tipo)
            .HasConversion<int>()
            .IsRequired();
    }
}
