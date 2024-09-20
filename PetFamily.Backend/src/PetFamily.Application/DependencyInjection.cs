using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Features.VolunteerManagement.Commands.AddFilesPet;
using PetFamily.Application.Features.VolunteerManagement.Commands.AddPet;
using PetFamily.Application.Features.VolunteerManagement.Commands.CreateVolunteer;
using PetFamily.Application.Features.VolunteerManagement.Commands.DeleteVolunteer;
using PetFamily.Application.Features.VolunteerManagement.Commands.UpdatePositionPet;
using PetFamily.Application.Features.VolunteerManagement.Commands.UpdateRequisites;
using PetFamily.Application.Features.VolunteerManagement.Commands.UpdateSocialLinks;
using PetFamily.Application.Features.VolunteerManagement.Commands.UpdateVolunteer;
using PetFamily.Application.Features.VolunteerManagement.Queries.GetVolunteersWithPagination;

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
            .AddScoped<GetVolunteersWithPaginationHandler>()
            .AddScoped<UpdatePetPositionHandler>()
            .AddScoped<UpdateVolunteerHandler>()
            .AddScoped<UpdateSocialLinksHandler>()
            .AddScoped<UpdateRequisitesHandler>()
            .AddScoped<DeleteVolunteerHandler>()
            .AddScoped<AddPetHandler>()
            .AddScoped<AddPhotosToPetHandler>();
    }
}