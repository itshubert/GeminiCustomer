using ErrorOr;
using GeminiCustomer.Domain.Common.Models;
using GeminiCustomer.Domain.Customers.Entities;
using GeminiCustomer.Domain.Customers.ValueObjects;

namespace GeminiCustomer.Domain.Customers;

public sealed class Customer : AggregateRoot<CustomerId>
{
    private readonly List<Address> _addresses = new List<Address>();

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public IReadOnlyList<Address> Addresses => _addresses.AsReadOnly();

    private Customer(
        CustomerId id,
        string firstName,
        string lastName,
        string email,
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

    public static ErrorOr<Customer> Create(
        CustomerId? id,
        string firstName,
        string lastName,
        string email,
        DateTimeOffset? createdAt,
        DateTimeOffset? updatedAt)
    {
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(firstName))
        {
            errors.Add(Error.Validation(
                code: "Customer.FirstName.Empty",
                description: "First name must not be empty."));
        }
        else if (firstName.Length > 50)
        {
            errors.Add(Error.Validation(
                code: "Customer.FirstName.TooLong",
                description: "First name must not exceed 50 characters."));
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            errors.Add(Error.Validation(
                code: "Customer.LastName.Empty",
                description: "Last name must not be empty."));
        }
        else if (lastName.Length > 50)
        {
            errors.Add(Error.Validation(
                code: "Customer.LastName.TooLong",
                description: "Last name must not exceed 50 characters."));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            errors.Add(Error.Validation(
                code: "Customer.Email.Empty",
                description: "Email must not be empty."));
        }
        else if (email.Length > 100)
        {
            errors.Add(Error.Validation(
                code: "Customer.Email.TooLong",
                description: "Email must not exceed 100 characters."));
        }
        else if (!IsValidEmail(email))
        {
            errors.Add(Error.Validation(
                code: "Customer.Email.Invalid",
                description: "Email format is invalid."));
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return new Customer(
            id: id ?? CustomerId.CreateUnique(),
            firstName: firstName,
            lastName: lastName,
            email: email,
            createdAt: createdAt ?? DateTimeOffset.UtcNow,
            updatedAt: updatedAt);
    }

    public ErrorOr<Address> CreateAddress(
        string addressLine1,
        string? addressLine2,
        string city,
        string state,
        string postCode,
        string country,
        bool isDefault = false)
    {
        // If this address should be default, unset the current default
        if (isDefault)
        {
            foreach (var existingAddress in _addresses.Where(a => a.IsDefault))
            {
                existingAddress.UnsetDefault();
            }
        }

        var addressResult = Address.Create(
            id: null, // Let Address.Create generate the ID
            customerId: Id,
            addressLine1: addressLine1,
            addressLine2: addressLine2,
            city: city,
            state: state,
            postCode: postCode,
            country: country,
            isDefault: isDefault,
            createdAt: null, // Let Address.Create set the timestamp
            updatedAt: null);

        if (addressResult.IsError)
        {
            return addressResult.Errors;
        }

        var address = addressResult.Value;
        _addresses.Add(address);
        UpdatedAt = DateTimeOffset.UtcNow;

        // TODO: Add domain event for address created
        // AddDomainEvent(new AddressAddedDomainEvent(Id, address.Id));

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

        // Unset all current defaults
        foreach (var address in _addresses.Where(a => a.IsDefault))
        {
            address.UnsetDefault();
        }

        targetAddress.SetAsDefault();
        UpdatedAt = DateTimeOffset.UtcNow;

        return Result.Success;
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}