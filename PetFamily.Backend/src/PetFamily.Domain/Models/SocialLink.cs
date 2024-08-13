namespace PetFamily.Domain.Models;

public class SocialLink
{
    public const int MIN_TEXT_NAME = 50;
    public const int MAX_TEXT_URL = 400;
    public string Name { get; }
    public string Url { get; }
}