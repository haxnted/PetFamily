using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteerManagement.Domain.ValueObjects;

public record PetPhysicalAttributes
{
    public double Weight { get; }
    public double Height { get; }

    private PetPhysicalAttributes(double weight, double height)
    {
        Weight = weight;
        Height = height;
    }

    public static Result<PetPhysicalAttributes, Error> Create(double weight, double height)
    {
        if (weight <= 0)
            return Errors.General.ValueIsInvalid("Weight");

        if (height <= 0)
            return Errors.General.ValueIsInvalid("Height");

        return new PetPhysicalAttributes(weight, height);
    }
}