using System.Text.Json.Serialization;

public class CustomServiceException : Exception
{
    [JsonPropertyName("method_name")]
    public string? MethodName { get; }
    public CustomServiceException(string methodName, string message) : base(message)
    {
        MethodName = methodName;
    }


}
public class HttpResponseException
{
    [JsonPropertyName("status")]
    public int? status { get; }

    [JsonPropertyName("internal_error")]
    public object? internalError { get; }

    [JsonPropertyName("message")]
    public string? message { get; }

    [JsonPropertyName("code")]
    public string? code { get; }

    [JsonPropertyName("name")]
    public string? name { get; }

    public HttpResponseException(int? status, object? internalError, string? message, string? code, string? name)
    {
        this.status = status;
        this.internalError = internalError;
        this.message = message;
        this.code = code;
        this.name = name;
    }
}
