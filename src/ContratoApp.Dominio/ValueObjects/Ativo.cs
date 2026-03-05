namespace ContratoApp.Dominio.ValueObjects;

public readonly struct Ativo
{
	public bool Value { get; }

	private Ativo(bool value)
	{
		Value = value;
	}

	public override string ToString() => Value ? "Sim" : "Não";

	public static Ativo Criar()
	{
		return new Ativo(true);
	}

	public static Ativo Ativar()
	{
		return new Ativo(true);
	}

	public static Ativo Inativar()
	{
		return new Ativo(false);
	}
}