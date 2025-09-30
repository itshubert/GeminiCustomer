using ErrorOr;
using GeminiCustomer.Domain.Common.Extensions;
using GeminiCustomer.Domain.Common.Models;
using GeminiCustomer.Domain.Customers.Entities;
using GeminiCustomer.Domain.Customers.ValueObjects;
using GeminiCustomer.Domain.Users.ValueObjects;

namespace GeminiCustomer.Domain.Customers;

// Alternative implementation using Fluent Validation Builder
public sealed class CustomerWithFluentValidation : AggregateRoot<CustomerId>
{
    private readonly List<Address> _addresses = new List<Address>();

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public IReadOnlyList<Address> Addresses => _addresses.AsReadOnly();

    private CustomerWithFluentValidation(
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

    public static ErrorOr<CustomerWithFluentValidation> Create(
        CustomerId? id,
        string firstName,
        string lastName,
        string email,
        DateTimeOffset? createdAt,
        DateTimeOffset? updatedAt)
    {
        var validation = new ValidationBuilder()
            .ValidateRequired(firstName, "FirstName", "First name")
            .ValidateMaxLength(firstName, "FirstName", "First name", CustomerValidationConstants.MaxNameLength)
            .ValidateRequired(lastName, "LastName", "Last name")
            .ValidateMaxLength(lastName, "LastName", "Last name", CustomerValidationConstants.MaxNameLength)
            .ValidateRequired(email, "Email", "Email")
            .ValidateMaxLength(email, "Email", "Email", CustomerValidationConstants.MaxEmailLength)
            .ValidateEmail(email);

        if (validation.HasErrors)
            return validation.Errors.ToList();

        return new CustomerWithFluentValidation(
            id: id ?? CustomerId.CreateUnique(),
            firstName: firstName,
            lastName: lastName,
            email: email,
            createdAt: createdAt ?? DateTimeOffset.UtcNow,
            updatedAt: updatedAt);
    }

    // Rest of the methods remain the same as in the original Customer class
}