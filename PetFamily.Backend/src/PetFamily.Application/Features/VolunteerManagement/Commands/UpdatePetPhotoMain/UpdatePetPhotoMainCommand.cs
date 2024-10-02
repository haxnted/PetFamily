using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Features.VolunteerManagement.Commands.UpdatePetPhotoMain;

public record UpdatePetPhotoMainCommand(Guid VolunteerId, Guid PetId, string FileName) : ICommand;