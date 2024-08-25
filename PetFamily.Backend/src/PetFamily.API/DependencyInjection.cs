using PetFamily.API.Validation;
using Serilog;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace PetFamily.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection collection)
    {
        collection.AddControllers();
        collection.AddEndpointsApiExplorer();
        collection.AddSwaggerGen();
        collection.AddSerilog();
        collection.AddFluentValidationAutoValidation(validation =>
        {
            validation.OverrideDefaultResultFactoryWith<CustomResultFactory>();
        });
        return collection;
    }
}