namespace PetFamily.Core.Dto;

public class PetDto
{
    public Guid Id { get; init; }
    public Guid VolunteerId { get; init; }
    public string NickName { get; init; } = string.Empty;
    public string GeneralDescription { get; init; } = string.Empty;
    public string HealthInformation { get; init; } = string.Empty;
    public Guid BreedId { get; init; }
    public Guid SpeciesId { get; init; }
    public AddressDto Address { get; init; } = default!;
    public double Weight { get; init; }
    public double Height { get; init; }
    public string PhoneNumber { get; init; } = string.Empty;
    public DateTime BirthDate { get; init; }
    public bool IsCastrated { get; init; }
    public bool IsVaccinated { get; init; }
    public int HelpStatus { get; init; }
    public int Position { get; init; }
    public DateTime DateCreated { get; init; }
    public IEnumerable<RequisiteDto> Requisites { get; init; } = default!;
    public IEnumerable<PetPhotoDto> Photos { get; init; } = default!;
    public bool IsDeleted { get; init; }
}
