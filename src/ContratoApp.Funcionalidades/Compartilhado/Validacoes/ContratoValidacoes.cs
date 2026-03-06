using Flunt.Notifications;
using Flunt.Validations;

namespace ContratoApp.Funcionalidades.Compartilhado.Validacoes;

public static class ContratoValidacoes
{
    public static Contract<Notification> Validar(Guid idCliente, decimal valor, DateTime dataInicio, DateTime? dataFim)
    {
        return new Contract<Notification>()
            .Requires()
            .IsFalse(idCliente == Guid.Empty, nameof(idCliente), "IdCliente é obrigatório.")
            .IsGreaterThan(valor, 0, nameof(valor), "Valor deve ser maior que zero.")
            .IsFalse(dataFim.HasValue && dataFim.Value < dataInicio, nameof(dataFim), "A data fim não pode ser anterior à data início.");
    }
}
