namespace kvFileY.Api.Services;

public class ServiceResult<T>(bool isSuccess, string message, T? data = default(T))
{
    public bool IsSuccess { get; set; } = isSuccess;
    public string Message { get; set; } = message;
    public T? Data { get; set; } = data;
}