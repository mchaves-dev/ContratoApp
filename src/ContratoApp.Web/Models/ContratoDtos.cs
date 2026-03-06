namespace ContratoApp.Web.Models;

public sealed record ContratoResponse(
    Guid Id,
    Guid IdCliente,
    int Tipo,
    decimal Valor,
    DateTime DataInicio,
    DateTime? DataFim,
    int Status,
    string Observacoes,
    DateTime CriadoEmUtc,
    DateTime? AtualizadaEmUtc);

public sealed record CriarContratoRequest(
    Guid IdCliente,
    int Tipo,
    decimal Valor,
    DateTime DataInicio,
    DateTime? DataFim,
    int Status,
    string Observacoes);
