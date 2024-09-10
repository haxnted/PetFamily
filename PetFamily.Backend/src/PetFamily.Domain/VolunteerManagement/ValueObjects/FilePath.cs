using CSharpFunctionalExtensions;
using PetFamily.Domain.Shared;

namespace PetFamily.Domain.VolunteerManagement.ValueObjects;

public record FilePath
{
    public const int MAX_LENGTH_NAME_FILE = 255;
    public string Path { get; }
    public string Extension => System.IO.Path.GetExtension(Path);

    private FilePath(string path) =>
        Path = path;

    public static Result<FilePath, Error> Create(string name, string extension)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > Constants.MIDDLE_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid("path");
        
        if (string.IsNullOrWhiteSpace(extension) || extension.Length > Constants.MIDDLE_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid("file-extension");
        
        var trimmedName = name.Trim();
        var trimmedExtension = extension.Trim();
        
        if (!trimmedExtension.StartsWith("."))
            trimmedExtension = "." + trimmedExtension;
        
        if (ContainsInvalidCharacters(trimmedName))
            return Errors.General.ValueIsInvalid("path");
        
        if (ContainsInvalidCharacters(trimmedExtension))
            return Errors.General.ValueIsInvalid("extension");
        
        var resultPath = $"{trimmedName}{trimmedExtension}";
        
        if (resultPath.Length > MAX_LENGTH_NAME_FILE)
            return Errors.General.ValueIsInvalid("path");
        
        return new FilePath(resultPath);
    }
    public static Result<FilePath, Error> Create(string fullPath)
    {
        if (string.IsNullOrWhiteSpace(fullPath))
            return Errors.General.ValueIsInvalid("file path");

        return new FilePath(fullPath);
    }
    
    private static bool ContainsInvalidCharacters(string input)
    {
        var invalidChars = System.IO.Path.GetInvalidFileNameChars();
        return input.IndexOfAny(invalidChars) >= 0;
    }
    
    public static implicit operator string(FilePath filePath) => filePath.Path;
}