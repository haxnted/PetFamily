namespace PetFamily.Infrastructure.Models;

public record FileUploadContent(
    Stream Stream,
    string BucketName,
    string ObjectName);