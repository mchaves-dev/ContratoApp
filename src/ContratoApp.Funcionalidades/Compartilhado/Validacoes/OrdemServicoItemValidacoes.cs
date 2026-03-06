using Flunt.Notifications;
using Flunt.Validations;

namespace ContratoApp.Funcionalidades.Compartilhado.Validacoes;

public static class OrdemServicoItemValidacoes
{
    public static Contract<Notification> Validar(Guid idOrdemServico, string descricao, int quantidade, decimal valorUnitario)
    {
        return new Contract<Notification>()
            .Requires()
            .IsFalse(idOrdemServico == Guid.Empty, nameof(idOrdemServico), "IdOrdemServico é obrigatório.")
            .IsNotNullOrWhiteSpace(descricao, nameof(descricao), "Descrição é obrigatória.")
            .IsGreaterThan(quantidade, 0, nameof(quantidade), "Quantidade deve ser maior que zero.")
            .IsGreaterOrEqualsThan(valorUnitario, 0, nameof(valorUnitario), "Valor unitário não pode ser negativo.");
    }
}
