namespace GeminiCustomer.Contracts;

public sealed record CreateCustomerRequest(
    string FirstName,
    string LastName,
    string Email);