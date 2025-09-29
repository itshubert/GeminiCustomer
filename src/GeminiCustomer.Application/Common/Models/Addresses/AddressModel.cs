using GeminiCustomer.Domain.Common.Enums;

namespace GeminiCustomer.Application.Common.Models.Addresses;

public sealed record AddressModel(
    Guid Id,
    Guid CustomerId,
    string AddressLine1,
    string? AddressLine2,
    string City,
    string State,
    string PostCode,
    CountryCode CountryCode,
    string CountryName
);