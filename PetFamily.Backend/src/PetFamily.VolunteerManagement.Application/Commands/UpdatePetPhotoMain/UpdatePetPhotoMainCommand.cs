using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerManagement.Application.Commands.UpdatePetPhotoMain;

public record UpdatePetPhotoMainCommand(Guid VolunteerId, Guid PetId, string FileName) : ICommand;