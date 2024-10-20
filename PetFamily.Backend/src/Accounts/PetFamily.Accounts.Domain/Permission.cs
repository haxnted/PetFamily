namespace PetFamily.Accounts.Domain;

public class Permission
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public List<RolePermission> RolePermissions { get; set; } = [];
}
