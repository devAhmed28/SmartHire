namespace SmartHire.Application.Common.Models;

public class Error
{
    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public string Code { get; }
    public string Message { get; }

    public static Error None => new(string.Empty, string.Empty);

    public static Error NullValue => new("Error.NullValue", "Null value was provided");

    public static Error Validation(string message) => new("Validation.Error", message);

    public static Error NotFound(string entity) => new("NotFound", $"{entity} was not found");

    public static Error Conflict(string message) => new("Conflict", message);

    public static Error Unauthorized(string message = "You are not authorized")
        => new("Unauthorized", message);

    public static Error Forbidden(string message = "Access denied")
        => new("Forbidden", message);

    public static Error BadRequest(string message) => new("BadRequest", message);

    public static Error Internal(string message = "An unexpected error occurred")
        => new("Internal", message);

    public override string ToString() => $"{Code}: {Message}";
}