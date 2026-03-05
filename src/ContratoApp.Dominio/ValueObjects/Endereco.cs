namespace ContratoApp.Dominio.ValueObjects;

public readonly struct Endereco
{
    public string Cidade { get; }
    public string Logradouro { get; }
    public string Bairro { get; }
    public string Numero { get; }
    public string Cep { get; }

    private Endereco(string cidade, string logradouro, string bairro, string numero, string cep)
    {
        Cidade = cidade.Trim().Normalize();
        Logradouro = logradouro.Trim().Normalize();
        Bairro = bairro.Trim().Normalize();
        Numero = numero.Trim().Normalize();
        Cep = cep.Trim().Normalize();
    }

    public override string ToString() => $"Rua {Logradouro},{Numero}, {Bairro}, {Cidade}- {Cep}";

    public static Endereco Criar(string cidade, string logradouro, string bairro, string numero, string cep)
    {
        return new Endereco(cidade, logradouro, bairro, numero, cep);
    }
}
