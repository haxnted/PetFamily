namespace PetFamily.API.Contracts;

public record AddPetFilesRequest(Guid PetId, IFormFileCollection Files, int IdxMainFile);