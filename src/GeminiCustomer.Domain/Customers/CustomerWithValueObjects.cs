using ErrorOr;
using GeminiCustomer.Domain.Common.Extensions;
using GeminiCustomer.Domain.Common.Models;
using GeminiCustomer.Domain.Customers.Entities;
using GeminiCustomer.Domain.Customers.ValueObjects;
using GeminiCustomer.Domain.Users.ValueObjects;

namespace GeminiCustomer.Domain.Customers;

// Alternative implementation using Value Objects (more robust)
public sealed class CustomerWithValueObjects : AggregateRoot<CustomerId>
{
    private readonly List<Address> _addresses = new List<Address>();

    public CustomerName FirstName { get; private set; }
    public CustomerName LastName { get; private set; }
    public EmailAddress Email { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public IReadOnlyList<Address> Addresses => _addresses.AsReadOnly();

    private CustomerWithValueObjects(
        CustomerId id,
        CustomerName firstName,
        CustomerName lastName,
        EmailAddress email,
        DateTimeOffset createdAt,
        DateTimeOffset? updatedAt)
        : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static ErrorOr<CustomerWithValueObjects> Create(
        CustomerId? id,
        string firstName,
        string lastName,
        string email,
        DateTimeOffset? createdAt,
        DateTimeOffset? updatedAt)
    {
        // Validate all inputs using value objects
        var firstNameResult = CustomerName.Create(firstName, "FirstName");
        var lastNameResult = CustomerName.Create(lastName, "LastName");
        var emailResult = EmailAddress.Create(email);

        // Collect all errors
        var errors = new List<Error>();
        if (firstNameResult.IsError) errors.AddRange(firstNameResult.Errors);
        if (lastNameResult.IsError) errors.AddRange(lastNameResult.Errors);
        if (emailResult.IsError) errors.AddRange(emailResult.Errors);

        if (errors.Count > 0)
            return errors;

        return new CustomerWithValueObjects(
            id: id ?? CustomerId.CreateUnique(),
            firstName: firstNameResult.Value,
            lastName: lastNameResult.Value,
            email: emailResult.Value,
            createdAt: createdAt ?? DateTimeOffset.UtcNow,
            updatedAt: updatedAt);
    }

    // Rest of the methods remain the same...
    public ErrorOr<Address> CreateAddress(
        string addressLine1,
        string? addressLine2,
        string city,
        string state,
        string postCode,
        string country,
        bool isDefault = false)
    {
        // Parse the country string to CountryCode enum
        if (!CountryCodeExtensions.TryParseCountry(country, out var countryCode))
        {
            return Error.Validation(
                code: "Customer.Address.Country.Invalid",
                description: $"'{country}' is not a valid country code or name.");
        }

        // If this address should be default, unset the current default
        if (isDefault)
        {
            foreach (var existingAddress in _addresses.Where(a => a.IsDefault))
            {
                existingAddress.UnsetDefault();
            }
        }

        var addressResult = Address.Create(
            id: null,
            customerId: Id,
            addressLine1: addressLine1,
            addressLine2: addressLine2,
            city: city,
            state: state,
            postCode: postCode,
            country: countryCode,
            isDefault: isDefault,
            createdAt: null,
            updatedAt: null);

        if (addressResult.IsError)
        {
            return addressResult.Errors;
        }

        var address = addressResult.Value;
        _addresses.Add(address);
        UpdatedAt = DateTimeOffset.UtcNow;

        return address;
    }

    public ErrorOr<Success> SetDefaultAddress(AddressId addressId)
    {
        var targetAddress = _addresses.FirstOrDefault(a => a.Id == addressId);
        if (targetAddress is null)
        {
            return Error.NotFound(
                code: "Customer.Address.NotFound",
                description: "Address not found for this customer.");
        }

        foreach (var address in _addresses.Where(a => a.IsDefault))
        {
            address.UnsetDefault();
        }

        targetAddress.SetAsDefault();
        UpdatedAt = DateTimeOffset.UtcNow;

        return Result.Success;
    }
}