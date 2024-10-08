using CSharpFunctionalExtensions;
using PetFamily.SharedKernel;

namespace PetFamily.VolunteerManagement.Domain.ValueObjects;

public record Position
{
    public int Value { get; }
    private Position(int value) => Value = value;

    public static Result<Position, Error> Create(int value)
    {
        if (value <= 0)
            return Errors.General.ValueIsInvalid(nameof(value));

        return Result.Success<Position, Error>(new Position(value));
    }
    public static implicit operator Position(int value) => new(value);

}