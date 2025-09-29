using GeminiCustomer.Application.Common.Models.Addresses;

namespace GeminiCustomer.Application.Common.Models.Customers;

public sealed record CustomerModel(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    List<AddressModel> Addresses);