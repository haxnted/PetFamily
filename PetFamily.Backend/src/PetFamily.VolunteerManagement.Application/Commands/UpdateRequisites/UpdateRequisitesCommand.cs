using PetFamily.Core.Abstractions;
using PetFamily.Core.Dto;

namespace PetFamily.VolunteerManagement.Application.Commands.UpdateRequisites;

public record UpdateRequisitesCommand(Guid Id, IEnumerable<RequisiteDto> Requisites) : ICommand;