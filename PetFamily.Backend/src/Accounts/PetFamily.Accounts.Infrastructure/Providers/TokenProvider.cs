using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PetFamily.Accounts.Application;
using PetFamily.Accounts.Domain;

namespace PetFamily.Accounts.Infrastructure.Providers;

public class TokenProvider(IOptions<JwtOptions> options) : ITokenProvider
{
    public string GenerateAccessToken(User user, CancellationToken cancellationToken)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.Key));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        Claim[] claims =
        [
            new(CustomClaims.Subject, user.Id.ToString()), new Claim(CustomClaims.Email, user.Email ?? ""),
        ];

        var jwtToken = new JwtSecurityToken(issuer: options.Value.Issuer,
            audience: options.Value.Audience,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(options.Value.ExpirationInMinutes)),
            signingCredentials: signingCredentials,
            claims: claims);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("234523tryhfgdsewtrsgdadwaerweeegswaeqeqweqweqewq"));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        return token;
    }
}
