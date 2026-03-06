using ContratoApp.Dominio.Enums;

namespace ContratoApp.Funcionalidades.OrdemServico.Compartilhado.Response;

public sealed record OrdemServicoResponse(
    Guid Id,
    Guid IdCliente,
    DateTime DataAbertura,
    DateTime? DataFechamento,
    EStatusOrdemServico Status,
    string Observacoes,
    DateTime CriadoEmUtc,
    DateTime? AtualizadaEmUtc);
