using PetFamily.Core.Abstractions;

namespace PetFamily.VolunteerManagement.Application.Queries.GetPetById;

public record GetPetByIdQuery(Guid PetId) : IQuery;