using Serilog;

namespace PetFamily.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddWeb(this IServiceCollection collection)
    {
        collection.AddControllers();
        collection.AddEndpointsApiExplorer();
        collection.AddSwaggerGen();
        collection.AddSerilog();
        return collection;
    }
}