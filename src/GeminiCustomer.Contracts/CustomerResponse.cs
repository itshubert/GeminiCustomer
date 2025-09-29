namespace GeminiCustomer.Contracts;

public sealed record CustomerResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    List<AddressResponse> Addresses);