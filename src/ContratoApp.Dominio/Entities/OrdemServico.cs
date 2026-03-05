using ContratoApp.Dominio.Enums;
using ContratoApp.Dominio.ValueObjects;

namespace ContratoApp.Dominio.Entities;

public sealed class OrdemServico : Entidade, IEntidadeAuditavel
{
    public Guid IdCliente { get; set; }
    public DateTime DataAbertura { get; set; }
    public DateTime? DataFechamento { get; set; }
    public EStatusOrdemServico Status { get; set; }
    public Observacao Observacoes { get; set; }
    public DateTime CriadoEmUtc { get; set; }
    public DateTime? AtualizadaEmUtc { get; set; }
}
