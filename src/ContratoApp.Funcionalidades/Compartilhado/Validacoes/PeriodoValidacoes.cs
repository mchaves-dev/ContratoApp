using Flunt.Notifications;
using Flunt.Validations;

namespace ContratoApp.Funcionalidades.Compartilhado.Validacoes;

public static class PeriodoValidacoes
{
    public static Contract<Notification> Validar(DateTime inicio, DateTime fim)
    {
        return new Contract<Notification>()
            .Requires()
            .IsFalse(inicio > fim, nameof(inicio), "A data de início não pode ser maior que a data fim.");
    }
}
