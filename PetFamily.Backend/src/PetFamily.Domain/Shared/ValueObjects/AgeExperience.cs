using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Shared.ValueObjects;

public record AgeExperience
{
    public int Years { get; }

    private AgeExperience(int years) => Years = years;
    
    public static Result<AgeExperience, Error> Create(int years)
    {
        if (years < 0)
            return Errors.General.ValueIsInvalid("Years");

        return new AgeExperience(years);
    }
}