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
    
    public static Result<PetPhysicalAttributes> Create(double weight, double height)
    {
        if (weight <= 0)
            return Result<PetPhysicalAttributes>.Failure("Weight must be greater than zero.");

        if (height <= 0)
            return Result<PetPhysicalAttributes>.Failure("Height must be greater than zero.");
        
        return Result<PetPhysicalAttributes>.Success(new PetPhysicalAttributes(weight, height));
    }
}