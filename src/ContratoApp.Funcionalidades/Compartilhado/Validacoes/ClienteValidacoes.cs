using Flunt.Notifications;
using Flunt.Validations;

namespace ContratoApp.Funcionalidades.Compartilhado.Validacoes;

public static class ClienteValidacoes
{
    public static Contract<Notification> ValidarDados(string nome, string sobrenome, string email, string cidade, string logradouro, string bairro, string numero, string cep)
    {
        return new Contract<Notification>()
            .Requires()
            .IsNotNullOrWhiteSpace(nome, nameof(nome), "Nome é obrigatório.")
            .IsNotNullOrWhiteSpace(sobrenome, nameof(sobrenome), "Sobrenome é obrigatório.")
            .IsNotNullOrWhiteSpace(email, nameof(email), "Email é obrigatório.")
            .IsEmail(email ?? string.Empty, nameof(email), "Email inválido.")
            .IsNotNullOrWhiteSpace(cidade, nameof(cidade), "Cidade é obrigatória.")
            .IsNotNullOrWhiteSpace(logradouro, nameof(logradouro), "Logradouro é obrigatório.")
            .IsNotNullOrWhiteSpace(bairro, nameof(bairro), "Bairro é obrigatório.")
            .IsNotNullOrWhiteSpace(numero, nameof(numero), "Número é obrigatório.")
            .IsNotNullOrWhiteSpace(cep, nameof(cep), "CEP é obrigatório.");
    }
}
