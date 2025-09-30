using ErrorOr;
using GeminiCustomer.Domain.Common.Models;

namespace GeminiCustomer.Domain.Customers.ValueObjects;

public sealed class CustomerName : ValueObject
{
    public string Value { get; private set; }

    private CustomerName(string value)
    {
        Value = value;
    }

    public static ErrorOr<CustomerName> Create(string name, string fieldName = "Name")
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Error.Validation(
                code: $"Customer.{fieldName}.Empty",
                description: $"{fieldName} must not be empty.");
        }

        if (name.Length > CustomerValidationConstants.MaxNameLength)
        {
            return Error.Validation(
                code: $"Customer.{fieldName}.TooLong",
                description: $"{fieldName} must not exceed {CustomerValidationConstants.MaxNameLength} characters.");
        }

        return new CustomerName(name.Trim());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public static implicit operator string(CustomerName customerName) => customerName.Value;
}