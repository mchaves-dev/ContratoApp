using FluentResults;
using Flunt.Notifications;

namespace ContratoApp.Funcionalidades.Compartilhado.Validacoes;

public static class FluntResultExtensions
{
    public static Result<T> ToFailedResult<T>(this Notifiable<Notification> notifiable)
    {
        var errors = notifiable.Notifications
            .Select(x => x.Message)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Select(message => (IError)new Error(message))
            .ToList();

        return Result.Fail<T>(errors);
    }
}
