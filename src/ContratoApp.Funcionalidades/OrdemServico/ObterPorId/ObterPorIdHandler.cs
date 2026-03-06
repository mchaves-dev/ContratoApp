using ContratoApp.Dominio.Handlers;
using ContratoApp.Funcionalidades.OrdemServico.Compartilhado.Response;
using ContratoApp.Infra.Database;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ContratoApp.Funcionalidades.OrdemServico.ObterPorId;

public interface IObterOrdemServicoPorIdHandler : IHandler
{
    Task<Result<OrdemServicoResponse>> HandleAsync(Guid id, CancellationToken cancellationToken);
}

public sealed class ObterOrdemServicoPorIdHandler(AppDbContext dbContext) : IObterOrdemServicoPorIdHandler
{
    public async Task<Result<OrdemServicoResponse>> HandleAsync(Guid id, CancellationToken cancellationToken)
    {
        var ordemServico = await dbContext.OrdemServicos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            .ConfigureAwait(false);

        if (ordemServico is null)
            return Result.Fail("Ordem de serviço não encontrada.");

        return Result.Ok(new OrdemServicoResponse(
            ordemServico.Id,
            ordemServico.IdCliente,
            ordemServico.DataAbertura,
            ordemServico.DataFechamento,
            ordemServico.Status,
            ordemServico.Observacoes.Valor,
            ordemServico.CriadoEmUtc,
            ordemServico.AtualizadaEmUtc));
    }
}
