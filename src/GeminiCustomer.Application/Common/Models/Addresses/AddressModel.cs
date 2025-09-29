namespace GeminiCustomer.Application.Common.Models.Addresses;

public sealed record AddressModel(
    Guid Id,
    Guid CustomerId,
    string AddressLine1,
    string? AddressLine2,
    string City,
    string State,
    string PostCode,
    string Country
);