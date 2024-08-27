using PetFamily.Domain.Shared.EntityIds;
using PetFamily.Domain.Shared.ValueObjects;

namespace PetFamily.Domain.Аggregate.Volunteer;

public class Volunteer : Shared.Entity<VolunteerId>
{
    private Volunteer(VolunteerId id) : base(id)
    {
    }

    public Volunteer(
        VolunteerId id,
        FullName fullName,
        Description generalDescription,
        AgeExperience ageExperience,
        PhoneNumber number,
        SocialLinksList socialLinksList,
        RequisitesList requisitesList) : base(id)
    {
        FullName = fullName;
        GeneralDescription = generalDescription;
        AgeExperience = ageExperience;
        PhoneNumber = number;
        SocialLinksList = socialLinksList;
        RequisitesList = requisitesList;
    }

    public FullName FullName { get; private set; }
    public Description GeneralDescription { get; private set; }
    public AgeExperience AgeExperience { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    private readonly List<Pet> _pets;
    public IReadOnlyList<Pet> Pets => _pets;
    public SocialLinksList SocialLinksList { get; private set; }
    public RequisitesList RequisitesList { get; private set; }

    public void UpdateSocialLinks(SocialLinksList list) =>
        SocialLinksList = list;

    public void UpdateRequisites(RequisitesList list) =>
        RequisitesList = list;

    public void UpdateMainInfo(FullName fullName, 
        Description generalDescription, 
        AgeExperience ageExperience,
        PhoneNumber number)
    {
        FullName = fullName;
        GeneralDescription = generalDescription;
        AgeExperience = ageExperience;
        PhoneNumber = number;
    }

    public int PetsAdoptedCount() =>
        _pets.Count(x => x.HelpStatus == HelpStatusPet.FoundHome);

    public int PetsFoundHomeQuantity() =>
        _pets.Count(x => x.HelpStatus == HelpStatusPet.LookingForHome);

    public int PetsUnderTreatmentCount() =>
        _pets.Count(x => x.HelpStatus == HelpStatusPet.NeedsHelp);
}