using GeminiCustomer.Domain.Common.Models;

namespace GeminiCustomer.Domain.Customers.ValueObjects;

public sealed class CustomerId : ValueObject
{
    public Guid Value { get; }

    private CustomerId(Guid value)
    {
        Value = value;
    }

    public static CustomerId CreateUnique() => new(Guid.NewGuid());

    public static CustomerId Create(Guid value) => new(value);

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();

    public CustomerId() { }
}