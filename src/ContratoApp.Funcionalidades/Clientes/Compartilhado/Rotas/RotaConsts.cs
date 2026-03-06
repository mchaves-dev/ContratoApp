namespace ContratoApp.Funcionalidades.Clientes.Compartilhado.Rotas;

public static class RotaConsts
{
    public const string RotaBase = "/api/clientes";
    public const string Criar = RotaBase;
    public const string Atualizar = RotaBase + "/{id:guid}";
    public const string ObterPorId = RotaBase + "/{id:guid}";
    public const string ObterAtivos = RotaBase + "/ativos";
    public const string ObterPorPeriodo = RotaBase + "/periodo";
}
