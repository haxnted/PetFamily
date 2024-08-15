namespace PetFamily.Domain.Shared;

public record class FullName
{
    public string Name { get; }
    public string Surname { get; }
    public string Patronymic { get; }

    private FullName(string name, string surname, string patronymic)
    {
        Name = name;
        Surname = surname;
        Patronymic = patronymic;
    }

    public static Result<FullName> Create(string name, string surname, string patronymic)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > Constants.MIN_TEXT_LENGTH)
            return Result<FullName>.Failure($"Name cannot be empty or more then {Constants.MIN_TEXT_LENGTH}");

        if (string.IsNullOrWhiteSpace(surname) || name.Length > Constants.MIN_TEXT_LENGTH)
            return Result<FullName>.Failure($"Surname cannot be empty or more then {Constants.MIN_TEXT_LENGTH}");

        if (string.IsNullOrWhiteSpace(surname) || name.Length > Constants.MIN_TEXT_LENGTH)
            return Result<FullName>.Failure($"Patronymic cannot be empty or more then {Constants.MIN_TEXT_LENGTH}");

        return Result<FullName>.Success(new FullName(name, surname, patronymic));
    }
}