using PetFamily.Application.FileProvider;

namespace PetFamily.Application.Volunteers.AddFilesPet;

public record AddPetFilesCommand(Guid VolunteerId, Guid PetId, IEnumerable<FileContent> Files, int IdxMainImage);