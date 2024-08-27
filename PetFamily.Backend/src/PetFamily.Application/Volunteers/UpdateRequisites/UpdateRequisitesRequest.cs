using PetFamily.Application.Dto;

namespace PetFamily.Application.Volunteers.UpdateRequisites;

public record UpdateRequisitesRequest(Guid Id, IEnumerable<RequisiteDto> Requisites);