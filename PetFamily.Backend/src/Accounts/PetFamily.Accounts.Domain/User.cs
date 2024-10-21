using Microsoft.AspNetCore.Identity;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.Accounts.Domain;

public class User : IdentityUser<Guid>
{
    public string PhotoPath { get; init; }
    public List<SocialLink> SocialLinks { get; set; } = [];
    public Guid RoleId { get; init; }
    public Role Role { get; init; }
}