namespace ContratoApp.Funcionalidades.Contrato.Compartilhado.Rotas;

public static class RotaConsts
{
    public const string RotaBase = "/api/contratos";
    public const string Criar = RotaBase;
    public const string Atualizar = RotaBase + "/{id:guid}";
    public const string ObterPorId = RotaBase + "/{id:guid}";
    public const string ObterAtivos = RotaBase + "/ativos";
    public const string ObterPorPeriodo = RotaBase + "/periodo";
}
