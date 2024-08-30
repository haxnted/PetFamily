namespace PetFamily.Infrastructure.Models;

public record FIleContent(
    Stream Stream,
    string BucketName,
    string ObjectName);