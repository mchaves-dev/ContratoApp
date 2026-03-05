namespace ContratoApp.Dominio;

public interface IEntidadeAuditavel
{
    DateTime CriadoEmUtc { get; set; }
    DateTime? AtualizadaEmUtc { get; set; }
}
