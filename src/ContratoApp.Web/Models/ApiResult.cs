namespace ContratoApp.Web.Models;

public sealed record ApiResult<T>(bool Success, T? Data, IReadOnlyCollection<string> Errors)
{
    public static ApiResult<T> Ok(T data) => new(true, data, []);
    public static ApiResult<T> Fail(params string[] errors) => new(false, default, errors);
}
