using Flunt.Notifications;
using Flunt.Validations;

namespace ContratoApp.Funcionalidades.Compartilhado.Validacoes;

public static class OrdemServicoValidacoes
{
    public static Contract<Notification> Validar(Guid idCliente, DateTime dataAbertura, DateTime? dataFechamento)
    {
        return new Contract<Notification>()
            .Requires()
            .IsFalse(idCliente == Guid.Empty, nameof(idCliente), "IdCliente é obrigatório.")
            .IsFalse(dataFechamento.HasValue && dataFechamento.Value < dataAbertura, nameof(dataFechamento), "A data de fechamento não pode ser anterior à data de abertura.");
    }
}
