namespace PetFamily.Application.Features.Volunteers.UpdatePositionPet;

public record UpdatePetPositionCommand(Guid VolunteerId, Guid PetId, int Position);