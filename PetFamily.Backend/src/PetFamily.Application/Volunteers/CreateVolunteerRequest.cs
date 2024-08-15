namespace PetFamily.Application.Volunteers;

public record class CreateVolunteerRequest(
    string Name,
    string Surname,
    string Patronymic,
    string Description,
    int AgeExperience,
    string Number);