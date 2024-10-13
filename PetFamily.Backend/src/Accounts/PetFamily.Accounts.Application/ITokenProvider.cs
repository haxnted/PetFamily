using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Application;

public interface ITokenProvider
{
    string GenerateAccessToken(User user, CancellationToken cancellationToken = default);
}
