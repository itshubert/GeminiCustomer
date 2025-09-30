namespace GeminiCustomer.Application.Common.Models.Users;

public sealed record UserModel(
    Guid Id,
    Guid CustomerId,
    string Username,
    bool IsActive,
    string? Token,
    string? RefreshToken,
    DateTimeOffset? TokenExpiry);