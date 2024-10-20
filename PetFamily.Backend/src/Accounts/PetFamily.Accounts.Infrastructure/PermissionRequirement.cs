using Microsoft.AspNetCore.Authorization;

namespace PetFamily.Accounts.Infrastructure;

public class PermissionRequirement : IAuthorizationRequirement
{
    public string Code { get; set; }
    public PermissionRequirement(string code)
    { 
        Code = code;
    }

}
