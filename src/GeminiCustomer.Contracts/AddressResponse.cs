namespace GeminiCustomer.Contracts;

/// <summary>
/// Address response with both country code and display name for frontend flexibility.
/// </summary>
public sealed record AddressResponse(
    Guid Id,
    Guid CustomerId,
    string AddressLine1,
    string? AddressLine2,
    string City,
    string State,
    string PostCode,
    string CountryCode,
    string CountryName
);