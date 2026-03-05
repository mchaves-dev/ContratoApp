using ContratoApp.Dominio.Enums;
using ContratoApp.Dominio.ValueObjects;

namespace ContratoApp.Dominio.Entities;

public sealed class Contrato : Entidade, IEntidadeAuditavel
{
    public Guid IdCliente { get; set; }
    public ETipoContrato Tipo { get; set; }
    public Dinheiro Valor { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public EStatusContrato Status { get; set; }
    public Observacao Observacoes { get; set; }
    public DateTime CriadoEmUtc { get; set; }
    public DateTime? AtualizadaEmUtc { get; set; }
}
