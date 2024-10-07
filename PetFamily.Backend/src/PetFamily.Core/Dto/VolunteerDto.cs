namespace PetFamily.Core.Dto;

public class VolunteerDto
{
    public Guid Id { get; init; }
    public FullNameDto FullName { get; init; } = default!;
    public string GeneralDescription { get; init; } = string.Empty;
    public int AgeExperience { get; init; }
    public string PhoneNumber { get; init; } = string.Empty;
    public IEnumerable<PetDto> Pets { get; init; } = default!;
    public IEnumerable<RequisiteDto> Requisites { get; init; } = default!;
    public bool IsDeleted { get; init; }
}
