using ContratoApp.Dominio.Enums;

namespace ContratoApp.Funcionalidades.Compartilhado.Status;

internal static class StatusClassifier
{
    public static EStatusContrato ClassificarContrato(DateTime dataInicio, DateTime? dataFim, EStatusContrato statusAtual)
    {
        if (statusAtual == EStatusContrato.Recebido)
            return EStatusContrato.Recebido;

        if (dataFim.HasValue && dataFim.Value.Date < DateTime.Today)
            return EStatusContrato.Atrasado;

        if (dataInicio.Date > DateTime.Today)
            return EStatusContrato.Agendado;

        return EStatusContrato.Pendente;
    }

    public static EStatusOrdemServico ClassificarOrdemServico(DateTime dataAbertura, DateTime? dataFechamento, EStatusOrdemServico statusAtual)
    {
        if (statusAtual == EStatusOrdemServico.Faturada)
            return EStatusOrdemServico.Faturada;

        if (dataFechamento.HasValue)
            return EStatusOrdemServico.Concluida;

        if (dataAbertura.Date < DateTime.Today)
            return EStatusOrdemServico.EmAndamento;

        return EStatusOrdemServico.Aberta;
    }
}

