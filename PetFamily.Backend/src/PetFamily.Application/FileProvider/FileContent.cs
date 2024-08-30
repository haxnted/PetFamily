namespace PetFamily.Application.FileProvider;

public record FIleContent(
    Stream Stream,
    string BucketName,
    string ObjectName);