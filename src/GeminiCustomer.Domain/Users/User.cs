using ErrorOr;
using GeminiCustomer.Domain.Common.Models;
using GeminiCustomer.Domain.Customers;
using GeminiCustomer.Domain.Customers.ValueObjects;
using GeminiCustomer.Domain.Users.ValueObjects;

namespace GeminiCustomer.Domain.Users;

public sealed class User : AggregateRoot<UserId>
{
    public CustomerId CustomerId { get; private set; }
    public Customer Customer { get; private set; } = null!;
    public string Username { get; private set; }
    public string PasswordHash { get; private set; }
    public string PasswordSalt { get; private set; }
    public bool IsActive { get; private set; }
    public string? Token { get; private set; }
    public string? RefreshToken { get; private set; }
    public DateTimeOffset? TokenExpiry { get; private set; }


    private User(
        UserId id,
        CustomerId customerId,
        string username,
        string passwordHash,
        string passwordSalt,
        bool isActive,
        string? token,
        string? refreshToken,
        DateTimeOffset? tokenExpiry)
        : base(id)
    {
        CustomerId = customerId;
        Username = username;
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
        IsActive = isActive;
        Token = token;
        RefreshToken = refreshToken;
        TokenExpiry = tokenExpiry;
    }

    public static User Create(
        UserId? id,
        CustomerId customerId,
        string username,
        string passwordHash,
        string passwordSalt,
        bool isActive,
        string? token,
        string? refreshToken,
        DateTimeOffset? tokenExpiry)
    {
        return new User(
            id ?? UserId.CreateUnique(),
            customerId,
            username,
            passwordHash,
            passwordSalt,
            isActive,
            token,
            refreshToken,
            tokenExpiry);
    }

    public void UpdateToken(string token, string? refreshToken, DateTimeOffset tokenExpiry)
    {
        Token = token;
        RefreshToken = refreshToken;
        TokenExpiry = tokenExpiry;
    }

    public void ChangePassword(string passwordHash, string passwordSalt)
    {
        PasswordHash = passwordHash;
        PasswordSalt = passwordSalt;
    }

    public void ExpireToken()
    {
        Token = null;
        RefreshToken = null;
        TokenExpiry = null;
    }

#pragma warning disable CS8618
    private User()
    {
    }
#pragma warning restore CS8618
}