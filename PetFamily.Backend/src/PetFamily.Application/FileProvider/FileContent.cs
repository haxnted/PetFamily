using PetFamily.Domain.VolunteerManagement.ValueObjects;

namespace PetFamily.Application.FileProvider;

public record FileContent(Stream Stream, FilePath File, string BucketName);