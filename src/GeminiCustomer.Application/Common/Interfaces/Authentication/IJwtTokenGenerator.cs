using System.Security.Claims;
using GeminiCustomer.Domain.Users;

namespace GeminiCustomer.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    Token GenerateToken(User user);
    Token GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}

public record Token(string TokenString, DateTimeOffset ExpiryDate);