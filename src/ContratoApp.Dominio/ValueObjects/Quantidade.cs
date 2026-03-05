namespace ContratoApp.Dominio.ValueObjects;

public readonly struct Quantidade
{
    public int Valor { get; }

    private Quantidade(int valor)
    {
        Valor = valor;
    }

    public override string ToString() => Valor.ToString();

    public static Quantidade Criar(int valor)
    {
        return new Quantidade(valor);
    }
}
