namespace ContratoApp.Dominio.ValueObjects;

public readonly struct NomePessoa
{
    public string Nome { get; }
    public string Sobrenome { get; }

    private NomePessoa(string nome, string sobrenome)
    {
        Nome = nome.Trim().Normalize();
        Sobrenome = sobrenome.Trim().Normalize();
    }

    public override string ToString() => $"{Nome} {Sobrenome}";

    public static NomePessoa Criar(string nome, string sobrenome)
    {
        return new NomePessoa(nome, sobrenome);
    }
}
