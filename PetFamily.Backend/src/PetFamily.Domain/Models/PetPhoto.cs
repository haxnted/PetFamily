namespace PetFamily.Domain.Models;

public class PetPhoto
{
    public Guid Id { get; }
    public string Path { get; }
    public bool IsImageMain { get; }
}