namespace GeminiCustomer.Contracts;

/// <summary>
/// Request to create a new address. Country can be provided as either a country code (e.g., "US") or full name (e.g., "United States").
/// </summary>
public sealed record CreateAddressRequest(
    string AddressLine1,
    string? AddressLine2,
    string City,
    string State,
    string PostCode,
    string Country,
    bool IsDefault);