using System.Net.Http.Json;
using ContratoApp.Web.Models;

namespace ContratoApp.Web.Services;

public sealed class ContratoApiClient(HttpClient httpClient)
{
    public async Task<IReadOnlyCollection<ClienteResponse>> ObterClientesAtivosAsync(CancellationToken cancellationToken = default)
        => await GetListAsync<ClienteResponse>("/api/clientes/ativos", cancellationToken);

    public async Task<IReadOnlyCollection<ClienteResponse>> ObterClientesAsync(CancellationToken cancellationToken = default)
        => await GetListAsync<ClienteResponse>(
            $"/api/clientes/periodo?inicio={DateTime.UtcNow.AddYears(-30):O}&fim={DateTime.UtcNow.AddYears(10):O}",
            cancellationToken);

    public async Task<ApiResult<ClienteResponse>> CriarClienteAsync(CriarClienteRequest request, CancellationToken cancellationToken = default)
        => await PostAsync<CriarClienteRequest, ClienteResponse>("/api/clientes", request, cancellationToken);

    public async Task<IReadOnlyCollection<ContratoResponse>> ObterContratosAtivosAsync(CancellationToken cancellationToken = default)
        => await GetListAsync<ContratoResponse>("/api/contratos/ativos", cancellationToken);

    public async Task<IReadOnlyCollection<ContratoResponse>> ObterContratosAsync(CancellationToken cancellationToken = default)
        => await GetListAsync<ContratoResponse>(
            $"/api/contratos/periodo?inicio={DateTime.UtcNow.AddYears(-30):O}&fim={DateTime.UtcNow.AddYears(10):O}",
            cancellationToken);

    public async Task<ApiResult<ContratoResponse>> CriarContratoAsync(CriarContratoRequest request, CancellationToken cancellationToken = default)
        => await PostAsync<CriarContratoRequest, ContratoResponse>("/api/contratos", request, cancellationToken);

    public async Task<ApiResult<ContratoResponse>> AtualizarContratoAsync(Guid id, AtualizarContratoRequest request, CancellationToken cancellationToken = default)
        => await PutAsync<AtualizarContratoRequest, ContratoResponse>($"/api/contratos/{id}", request, cancellationToken);

    public async Task<IReadOnlyCollection<OrdemServicoResponse>> ObterOrdensAtivasAsync(CancellationToken cancellationToken = default)
        => await GetListAsync<OrdemServicoResponse>("/api/ordens-servico/ativos", cancellationToken);

    public async Task<IReadOnlyCollection<OrdemServicoResponse>> ObterOrdensAsync(CancellationToken cancellationToken = default)
        => await GetListAsync<OrdemServicoResponse>(
            $"/api/ordens-servico/periodo?inicio={DateTime.UtcNow.AddYears(-30):O}&fim={DateTime.UtcNow.AddYears(10):O}",
            cancellationToken);

    public async Task<ApiResult<OrdemServicoResponse>> CriarOrdemServicoAsync(CriarOrdemServicoRequest request, CancellationToken cancellationToken = default)
        => await PostAsync<CriarOrdemServicoRequest, OrdemServicoResponse>("/api/ordens-servico", request, cancellationToken);

    public async Task<ApiResult<OrdemServicoResponse>> AtualizarOrdemServicoAsync(Guid id, AtualizarOrdemServicoRequest request, CancellationToken cancellationToken = default)
        => await PutAsync<AtualizarOrdemServicoRequest, OrdemServicoResponse>($"/api/ordens-servico/{id}", request, cancellationToken);

    public async Task<IReadOnlyCollection<OrdemServicoItemResponse>> ObterItensPorOrdemServicoAsync(Guid idOrdemServico, CancellationToken cancellationToken = default)
        => await GetListAsync<OrdemServicoItemResponse>($"/api/ordens-servico/itens/ordem/{idOrdemServico}", cancellationToken);

    public async Task<ApiResult<OrdemServicoItemResponse>> AdicionarItemAsync(AdicionarOrdemServicoItemRequest request, CancellationToken cancellationToken = default)
        => await PostAsync<AdicionarOrdemServicoItemRequest, OrdemServicoItemResponse>("/api/ordens-servico/itens", request, cancellationToken);

    private async Task<IReadOnlyCollection<TResponse>> GetListAsync<TResponse>(string url, CancellationToken cancellationToken)
    {
        using var response = await httpClient.GetAsync(url, cancellationToken).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
            return [];

        return await response.Content.ReadFromJsonAsync<List<TResponse>>(cancellationToken).ConfigureAwait(false) ?? [];
    }

    private async Task<ApiResult<TResponse>> PostAsync<TRequest, TResponse>(string url, TRequest request, CancellationToken cancellationToken)
    {
        try
        {
            using var response = await httpClient.PostAsJsonAsync(url, request, cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken).ConfigureAwait(false);
                return data is null
                    ? ApiResult<TResponse>.Fail("A API retornou resposta vazia.")
                    : ApiResult<TResponse>.Ok(data);
            }

            var payload = await response.Content.ReadFromJsonAsync<ErrorPayload>(cancellationToken).ConfigureAwait(false);
            if (payload is not null && payload.Erros.Length > 0)
                return new ApiResult<TResponse>(false, default, payload.Erros);

            return ApiResult<TResponse>.Fail($"Erro HTTP {(int)response.StatusCode}.");
        }
        catch (Exception ex)
        {
            return ApiResult<TResponse>.Fail(ex.Message);
        }
    }

    private async Task<ApiResult<TResponse>> PutAsync<TRequest, TResponse>(string url, TRequest request, CancellationToken cancellationToken)
    {
        try
        {
            using var response = await httpClient.PutAsJsonAsync(url, request, cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken).ConfigureAwait(false);
                return data is null
                    ? ApiResult<TResponse>.Fail("A API retornou resposta vazia.")
                    : ApiResult<TResponse>.Ok(data);
            }

            var payload = await response.Content.ReadFromJsonAsync<ErrorPayload>(cancellationToken).ConfigureAwait(false);
            if (payload is not null && payload.Erros.Length > 0)
                return new ApiResult<TResponse>(false, default, payload.Erros);

            return ApiResult<TResponse>.Fail($"Erro HTTP {(int)response.StatusCode}.");
        }
        catch (Exception ex)
        {
            return ApiResult<TResponse>.Fail(ex.Message);
        }
    }
}


