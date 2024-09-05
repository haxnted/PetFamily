using PetFamily.Application.Dto;

namespace PetFamily.Application.Features.Volunteers.UpdateRequisites;

public record UpdateRequisitesCommand(Guid Id, IEnumerable<RequisiteDto> Requisites);