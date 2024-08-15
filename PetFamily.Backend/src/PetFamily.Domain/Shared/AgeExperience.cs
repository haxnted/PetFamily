namespace PetFamily.Domain.Shared;

public record class AgeExperience
{
    public int Years { get; }
    private AgeExperience(int years)
    {
        Years = years;
    }

    public static Result<AgeExperience> Create(int years)
    {
        if (years < 0 )
            return Result<AgeExperience>.Failure("Years and months must both be zero or positive.");
        
        return Result<AgeExperience>.Success(new AgeExperience(years));
    }
}
