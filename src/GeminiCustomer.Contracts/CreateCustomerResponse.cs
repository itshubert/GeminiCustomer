namespace GeminiCustomer.Contracts;

public sealed record CreateCustomerResponse(
    CustomerResponse Customer,
    UserResponse? User);