namespace GeminiCustomer.Contracts;

public sealed record UserResponse(
    Guid Id,
    Guid CustomerId,
    string Username,
    bool IsActive,
    string? Token,
    string? RefreshToken,
    DateTimeOffset? TokenExpiry);