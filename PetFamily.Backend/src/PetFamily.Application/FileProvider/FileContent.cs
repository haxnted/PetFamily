namespace PetFamily.Application.FileProvider;

public record FileContent(
    Stream Stream,
    string ObjectName);