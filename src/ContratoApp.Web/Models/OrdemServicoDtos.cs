namespace ContratoApp.Web.Models;

public sealed record OrdemServicoResponse(
    Guid Id,
    Guid IdCliente,
    DateTime DataAbertura,
    DateTime? DataFechamento,
    int Status,
    string Observacoes,
    DateTime CriadoEmUtc,
    DateTime? AtualizadaEmUtc);

public sealed record CriarOrdemServicoRequest(
    Guid IdCliente,
    DateTime DataAbertura,
    DateTime? DataFechamento,
    int Status,
    string Observacoes);
