using PetFamily.VolunteerManagement.Domain.ValueObjects;

namespace PetFamily.Core.Providers.FileProvider;

public record FileContent(Stream Stream, FilePath File, string BucketName);