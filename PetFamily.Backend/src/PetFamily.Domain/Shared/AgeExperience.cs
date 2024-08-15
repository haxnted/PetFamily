namespace PetFamily.Domain.Shared;

public record class AgeExperience
{
    public int Years { get; }
    public int Months { get; }

    private AgeExperience(int years, int months)
    {
        Years = years;
        Months = months;
    }

    public static Result<AgeExperience> Create(int years, int months)
    {
        if (years < 0 || months < 0)
            return Result<AgeExperience>.Failure("Years and months must both be zero or positive.");

        if (months > 12)
            return Result<AgeExperience>.Failure("Count months cannot be more than 12.");

        if (months == 12)
            return Result<AgeExperience>.Success(new AgeExperience(years + 1, 0));

        if (years == 0 && months == 0)
            return Result<AgeExperience>.Failure("Years and months cannot both be zero.");

        return Result<AgeExperience>.Success(new AgeExperience(years, months));
    }
}
