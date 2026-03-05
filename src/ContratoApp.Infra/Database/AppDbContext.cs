using ContratoApp.Dominio.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContratoApp.Infra.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
	public DbSet<Cliente> Clientes { get; set; }
	public DbSet<Contrato> Contratos { get; set; }
	public DbSet<OrdemServico> OrdemServicos { get; set; }
	public DbSet<OrdemServicoItem> OrdemServicoItens { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
	}
}
