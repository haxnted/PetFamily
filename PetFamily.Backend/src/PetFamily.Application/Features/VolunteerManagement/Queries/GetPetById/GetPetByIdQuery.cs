using PetFamily.Application.Abstractions;

namespace PetFamily.Application.Features.VolunteerManagement.Queries.GetPetById;

public record GetPetByIdQuery(Guid PetId) : IQuery;