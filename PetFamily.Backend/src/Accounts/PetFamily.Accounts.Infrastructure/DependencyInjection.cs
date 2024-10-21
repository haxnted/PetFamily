using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Accounts.Application;
using PetFamily.Accounts.Domain;
using PetFamily.Accounts.Infrastructure.Providers;

namespace PetFamily.Accounts.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountsInfrastructure(
        this IServiceCollection collection, IConfiguration configuration)
    {
        collection.Configure<JwtOptions>(configuration.GetSection(JwtOptions.JWT));
        collection.AddTransient<ITokenProvider, TokenProvider>();

        collection.AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
            })
            .AddEntityFrameworkStores<AuthorizationDbContext>();

        collection.AddScoped<AuthorizationDbContext>();
        collection.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();
        collection.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        return collection;
    }
}
