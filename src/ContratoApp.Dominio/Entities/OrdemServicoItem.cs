using ContratoApp.Dominio.Enums;
using ContratoApp.Dominio.ValueObjects;

namespace ContratoApp.Dominio.Entities;

public sealed class OrdemServicoItem
{
    public Guid IdOrdemServico { get; set; }
    public Descricao Descricao { get; set; }
    public Quantidade Quantidade { get; set; }
    public Dinheiro ValorUnitario { get; set; }
    public ETipoOrdemServicoItem Tipo { get; set; }
}
