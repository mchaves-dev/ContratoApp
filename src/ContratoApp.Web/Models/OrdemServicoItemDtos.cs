namespace ContratoApp.Web.Models;

public sealed record OrdemServicoItemResponse(
    Guid IdOrdemServico,
    string Descricao,
    int Quantidade,
    decimal ValorUnitario,
    int Tipo);

public sealed record AdicionarOrdemServicoItemRequest(
    Guid IdOrdemServico,
    string Descricao,
    int Quantidade,
    decimal ValorUnitario,
    int Tipo);
