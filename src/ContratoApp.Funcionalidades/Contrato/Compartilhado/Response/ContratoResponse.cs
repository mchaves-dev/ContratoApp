using ContratoApp.Dominio.Enums;

namespace ContratoApp.Funcionalidades.Contrato.Compartilhado.Response;

public sealed record ContratoResponse(
    Guid Id,
    Guid IdCliente,
    ETipoContrato Tipo,
    decimal Valor,
    DateTime DataInicio,
    DateTime? DataFim,
    EStatusContrato Status,
    string Observacoes,
    DateTime CriadoEmUtc,
    DateTime? AtualizadaEmUtc);
