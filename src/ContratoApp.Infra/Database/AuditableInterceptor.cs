using ContratoApp.Dominio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ContratoApp.Infra.Database;

public class AuditableInterceptor : SaveChangesInterceptor
{
	public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
		DbContextEventData eventData,
		InterceptionResult<int> result,
		CancellationToken cancellationToken = default)
	{
		var context = eventData.Context!;
		var entries = context.ChangeTracker.Entries<IEntidadeAuditavel>();

		foreach (var entry in entries)
		{
			if (entry.State == EntityState.Added)
			{
				entry.Entity.CriadoEmUtc = DateTime.UtcNow;
			}
			else if (entry.State == EntityState.Modified)
			{
				entry.Entity.AtualizadaEmUtc = DateTime.UtcNow;
			}
		}

		return await base.SavingChangesAsync(eventData, result, cancellationToken);
	}
}
