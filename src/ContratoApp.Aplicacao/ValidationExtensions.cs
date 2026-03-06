using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;

namespace ContratoApp.Aplicacao;

public static class ValidationExtensions
{
    public static string ToFormattedErrorMessage(this ValidationResult validationResult)
    {
        var memberNames = validationResult.MemberNames?.ToArray() ?? [];
        var member = memberNames.Length > 0 ? string.Join(",", memberNames) : "Validation";
        return $"{member}: {validationResult.ErrorMessage}";
    }

    public static void LogValidationError<T>(this ILogger<T> logger, ValidationResult validationResult, string contextMessage)
    {
        logger.LogWarning("{ContextMessage}: {Error}", contextMessage, validationResult.ToFormattedErrorMessage());
    }
}
