using PetFamily.Application.Dto;

namespace PetFamily.Application.Features.VolunteerManagement.Commands.UpdateRequisites;

public record UpdateRequisitesCommand(Guid Id, IEnumerable<RequisiteDto> Requisites);