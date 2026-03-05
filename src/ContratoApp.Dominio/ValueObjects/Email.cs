namespace ContratoApp.Dominio.ValueObjects;

public readonly struct Email
{
    public string Endereco { get; }

    private Email(string endereco)
    {
        Endereco = endereco.Trim().Normalize();
    }

    public override string ToString() => Endereco;

    public static Email Criar(string endereco)
    {
        return new Email(endereco);
    }
}
