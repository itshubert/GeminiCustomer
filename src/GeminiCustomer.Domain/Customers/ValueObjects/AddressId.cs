using GeminiCustomer.Domain.Common.Models;

namespace GeminiCustomer.Domain.Customers.ValueObjects;

public sealed class AddressId : ValueObject
{
    public Guid Value { get; }

    private AddressId(Guid value)
    {
        Value = value;
    }

    public static AddressId Create(Guid? value = null)
    {
        return new AddressId(value ?? Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}