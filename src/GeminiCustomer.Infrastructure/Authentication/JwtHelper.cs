using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace GeminiCustomer.Infrastructure.Authentication;

public class JwtHelper
{
    public static TokenValidationParameters GetTokenValidationParameters(
        string issuer,
        string[] audiences,
        string secret,
        bool ignoreExpiration = false) => new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = !ignoreExpiration,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudiences = audiences,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
        };
}