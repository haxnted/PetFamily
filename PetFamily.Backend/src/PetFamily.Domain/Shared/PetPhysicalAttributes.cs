using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Shared;

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
            return Errors.General.ValueIsInvalid("Weight must be greater than zero.");

        if (height <= 0)
            return Errors.General.ValueIsInvalid("Height must be greater than zero.");
        
        return new PetPhysicalAttributes(weight, height);
    }
}