namespace PetFamily.Domain.Shared;

public record Error
{
    private const string SEPARATOR = "||";
    public string Code { get; }
    public string Message { get; }
    public ErrorType Type { get; }
    public string? InvalidField { get; }

    private Error(string code, string message, ErrorType type, string? invalidField = null)
    {
        Code = code;
        Message = message;
        Type = type;
        InvalidField = invalidField;
    }

    public static Error Validation(string code, string message, string? invalidField = null) =>
        new(code, message, ErrorType.Validation, invalidField);

    public static Error NotFound(string code, string message, string? invalidField = null) =>
        new(code, message, ErrorType.NotFound, invalidField);

    public static Error Failure(string code, string message, string? invalidField = null) =>
        new(code, message, ErrorType.Failure, invalidField);

    public static Error Conflict(string code, string message, string? invalidField = null) =>
        new(code, message, ErrorType.Conflict, invalidField);

    public string Serialize() => string.Join(SEPARATOR, Code, Message, Type);

    public static Error Deserialize(string serialized)
    {
        var parts = serialized.Split(SEPARATOR);
        if (parts.Length < 2)
            throw new InvalidOperationException("invalid serialize format");

        if (Enum.TryParse<ErrorType>(parts[2], out var type) == false)
            throw new InvalidOperationException("invalid serialize format");

        return new Error(parts[0], parts[1], type);
    }
    public ErrorList ToErrorList() => new([this]);
}

public enum ErrorType
{
    Validation,
    NotFound,
    Failure,
    Conflict
}