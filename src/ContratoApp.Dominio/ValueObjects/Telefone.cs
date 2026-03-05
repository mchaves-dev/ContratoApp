namespace ContratoApp.Dominio.ValueObjects;

public readonly struct Telefone
{
    public string DD { get; }
    public string Numero { get; }

    private Telefone(string dd, string numero)
    {
        DD = numero.Trim().Normalize();
        Numero = numero.Trim().Normalize();
    }

    public override string ToString() => $"({DD}) {Numero}";

    public static Telefone Criar(string dd, string numero)
    {
        return new Telefone(dd, numero);
    }
}
