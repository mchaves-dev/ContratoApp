namespace ContratoApp.Funcionalidades.Clientes.Compartilhado.Response;

public sealed record ClienteResponse(
    Guid Id,
    string Nome,
    string Sobrenome,
    string Email,
    string Cidade,
    string Logradouro,
    string Bairro,
    string Numero,
    string Cep,
    bool Ativo,
    DateTime CriadoEmUtc);
