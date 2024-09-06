using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Features.Volunteers.AddFilesPet;
using PetFamily.Application.Features.Volunteers.AddPet;
using PetFamily.Application.Features.Volunteers.CreateVolunteer;
using PetFamily.Application.Features.Volunteers.DeleteVolunteer;
using PetFamily.Application.Features.Volunteers.UpdatePositionPet;
using PetFamily.Application.Features.Volunteers.UpdateRequisites;
using PetFamily.Application.Features.Volunteers.UpdateSocialLinks;
using PetFamily.Application.Features.Volunteers.UpdateVolunteer;

namespace PetFamily.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection collection)
    {
        return collection.AddHandlers()
            .AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
    }

    private static IServiceCollection AddHandlers(this IServiceCollection collection)
    {
        return collection.AddScoped<CreateVolunteerHandler>()
            .AddScoped<UpdatePetPositionHandler>()
            .AddScoped<UpdateVolunteerHandler>()
            .AddScoped<UpdateSocialLinksHandler>()
            .AddScoped<UpdateRequisitesHandler>()
            .AddScoped<DeleteVolunteerHandler>()
            .AddScoped<AddPetHandler>()
            .AddScoped<AddPhotosToPetHandler>();
    }
}