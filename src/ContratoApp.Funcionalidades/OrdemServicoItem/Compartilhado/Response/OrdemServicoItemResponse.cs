using ContratoApp.Dominio.Enums;

namespace ContratoApp.Funcionalidades.OrdemServicoItem.Compartilhado.Response;

public sealed record OrdemServicoItemResponse(
    Guid IdOrdemServico,
    string Descricao,
    int Quantidade,
    decimal ValorUnitario,
    ETipoOrdemServicoItem Tipo);
