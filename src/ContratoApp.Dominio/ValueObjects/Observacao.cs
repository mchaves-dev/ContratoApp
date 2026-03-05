namespace ContratoApp.Dominio.ValueObjects;

public readonly struct Observacao
{
    public string Valor { get; }

    private Observacao(string valor)
    {
        Valor = valor;
    }

    public override string ToString() => Valor;

    public static Observacao Criar(string valor)
    {
        return new Observacao(valor);
    }
}
