using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.VolunteerManagement.ValueObjects;

public record SerialNumber
{
    public int Value { get; }
    public SerialNumber(int value) => Value = value;

    public static Result<SerialNumber, Error> Create(int value)
    {
        if (value <= 0)
            return Errors.General.ValueIsInvalid(nameof(value));

        return Result.Success<SerialNumber, Error>(new SerialNumber(value));
    }
    public static implicit operator SerialNumber(int value) => new(value);
    public static implicit operator int(SerialNumber serialNumber) => serialNumber.Value;
}