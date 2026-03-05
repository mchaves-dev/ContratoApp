namespace ContratoApp.Dominio.Entities;

public abstract class Entidade
{
    public Entidade()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; }
}
