namespace PetFamily.Domain.Shared;

public class Constants
{
    public const int MIN_TEXT_LENGTH = 50;
    public const int MIDDLE_TEXT_LENGTH = 100;
    public const int MAX_TEXT_LENGTH = 500;
    public const int EXTRA_TEXT_LENGTH = 2000;

    public static readonly string[] SUPPORTED_IMAGES_EXTENSIONS = [".jpg", ".png", ".jpeg"];
    
    public static readonly string BUCKET_NAME_FOR_PET_IMAGES = "pet-images";
}