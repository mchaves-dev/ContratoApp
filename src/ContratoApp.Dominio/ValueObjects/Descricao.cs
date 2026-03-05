namespace ContratoApp.Dominio.ValueObjects;

public readonly struct Descricao
{
    public string Valor { get; }

    private Descricao(string valor)
    {
        Valor = valor;
    }

    public override string ToString() => Valor;

    public static Descricao Criar(string valor)
    {
        return new Descricao(valor);
    }
}
