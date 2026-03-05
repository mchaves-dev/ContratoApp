namespace ContratoApp.Dominio.ValueObjects;

public readonly struct Dinheiro
{
    public decimal Valor { get; }

    private Dinheiro(decimal valor)
    {
        Valor = valor;
    }

    public override string ToString() => Valor.ToString();

    public static Dinheiro Criar(decimal valor)
    {
        return new Dinheiro(valor);
    }
}
