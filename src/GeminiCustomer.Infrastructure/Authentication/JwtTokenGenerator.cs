using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using GeminiCustomer.Application.Common.Interfaces.Authentication;
using GeminiCustomer.Application.Common.Interfaces.Services;
using GeminiCustomer.Domain.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GeminiCustomer.Infrastructure.Authentication;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly int keySize = 64;

    public JwtTokenGenerator(IDateTimeProvider dateTimeProvider, IOptions<JwtSettings> jwtOptions)
    {
        _dateTimeProvider = dateTimeProvider;
        _jwtSettings = jwtOptions.Value;
    }

    public Token GenerateToken(User user)
    {
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)), SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.Value.ToString()),
            new(JwtRegisteredClaimNames.GivenName, user.Customer.FirstName),
            new(JwtRegisteredClaimNames.FamilyName, user.Customer.LastName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("customer_id", user.Customer.Id.Value.ToString())
        };

        // claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));

        var securityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audiences.First(),
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: signingCredentials);

        var accessTokenStr = new JwtSecurityTokenHandler().WriteToken(securityToken);

        return new Token(accessTokenStr, securityToken.ValidTo);
    }

    public Token GenerateRefreshToken()
    {
        var tokenBytes = RandomNumberGenerator.GetBytes(keySize);
        // var expiryDays = _dateTimeProvider.UtcNow.AddDays(_jwtSettings.refreshTokenExpiryDays);
        var expiryDays = DateTime.UtcNow.AddDays(_jwtSettings.refreshTokenExpiryDays);

        return new Token(Convert.ToHexString(tokenBytes), expiryDays);
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParams = JwtHelper.GetTokenValidationParameters(
            _jwtSettings.Issuer,
            _jwtSettings.Audiences,
            _jwtSettings.Secret,
            true);

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParams, out SecurityToken securityToken);

        if (!IsJwtWithValidSecurityAlgorithm(securityToken))
        {
            return null;
        }

        return principal;
    }

    private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
    {
        return (
            validatedToken is JwtSecurityToken jwtSecurityToken) &&
            jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
            StringComparison.InvariantCultureIgnoreCase);
    }
}