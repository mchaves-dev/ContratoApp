using ContratoApp.Dominio.ValueObjects;

namespace ContratoApp.Dominio.Entities;

public sealed class Cliente : Entidade, IEntidadeAuditavel
{
    public NomePessoa Nome { get; set; }
    public Email Email { get; set; }
    public Endereco Endereco { get; set; }
    public DateTime CriadoEmUtc { get; set; }
    public DateTime? AtualizadaEmUtc { get; set; }
}
