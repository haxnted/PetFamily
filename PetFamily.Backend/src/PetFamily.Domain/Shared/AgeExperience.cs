using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Shared;

public record AgeExperience
{
    public int Years { get; }
    private AgeExperience(int years)
    {
        Years = years;
    }

    public static Result<AgeExperience, Error> Create(int years)
    {
        if (years < 0 )
            return Errors.General.ValueIsInvalid("Years and months must both be zero or positive.");
        
        return new AgeExperience(years);
    }
}
