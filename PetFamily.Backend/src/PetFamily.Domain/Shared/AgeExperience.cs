namespace PetFamily.Domain.Shared;

public record class AgeExperience
{
    public int Years { get; }

    private AgeExperience(int years, int months)
    {
        Years = years;
    }

    public static Result<AgeExperience> Create(int years, int months)
    {
        if (years < 0 )
            return Result<AgeExperience>.Failure("Years and months must both be zero or positive.");
        
        return Result<AgeExperience>.Success(new AgeExperience(years, months));
    }
}
