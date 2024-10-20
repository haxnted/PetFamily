namespace PetFamily.SharedKernel;

public class Permissions
{
    public static class Volunteer
    {
        public const string Update = "volunteer.update";
        public const string Delete = "volunteer.delete";
    }

    public static class Participant
    {
        public const string Create = "volunteer.create";
    }

    public static class Admin
    {
        public const string Create = "species.create";
        public const string Update = "species.update";
        public const string Delete = "species.delete";
    }
}
