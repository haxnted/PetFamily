namespace PetFamily.Application.Volunteers.CreateVolunteer;

public record class CreateVolunteerRequest(
    string Name,
    string Surname,
    string Patronymic,
    string Description,
    int AgeExperience,
    string Number);