namespace ContratoApp.Funcionalidades.OrdemServicoItem.Compartilhado.Rotas;

public static class RotaConsts
{
    public const string RotaBase = "/api/ordens-servico/itens";
    public const string Adicionar = RotaBase;
    public const string Atualizar = RotaBase;
    public const string ObterPorOrdemServico = RotaBase + "/ordem/{idOrdemServico:guid}";
    public const string ObterPorId = RotaBase + "/por-id";
    public const string Remover = RotaBase;
}
