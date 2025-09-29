namespace GeminiCustomer.Contracts;

public sealed record AddressResponse(
    Guid Id,
    Guid CustomerId,
    string AddressLine1,
    string? AddressLine2,
    string City,
    string State,
    string PostCode,
    string Country
);