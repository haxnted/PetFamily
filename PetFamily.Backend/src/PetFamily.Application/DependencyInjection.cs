using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Volunteers.AddFilesPet;
using PetFamily.Application.Volunteers.AddPet;
using PetFamily.Application.Volunteers.CreateVolunteer;
using PetFamily.Application.Volunteers.DeleteVolunteer;
using PetFamily.Application.Volunteers.UpdateRequisites;
using PetFamily.Application.Volunteers.UpdateSocialLinks;
using PetFamily.Application.Volunteers.UpdateVolunteer;

namespace PetFamily.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection collection)
    {
        collection.AddScoped<CreateVolunteerHandler>();
        collection.AddScoped<UpdateVolunteerHandler>();
        collection.AddScoped<UpdateSocialLinksHandler>();
        collection.AddScoped<UpdateRequisitesHandler>();
        collection.AddScoped<DeleteVolunteerHandler>();
        collection.AddScoped<AddPetHandler>();
        collection.AddScoped<AddPetFilesHandler>();
        collection.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        return collection;
    }
}