using ErrorOr;
using GeminiCustomer.Domain.Addresses;
using GeminiCustomer.Domain.Common.Models;
using GeminiCustomer.Domain.Customers.ValueObjects;

namespace GeminiCustomer.Domain.Customers;

public sealed class Customer : AggregateRoot<CustomerId>
{
    private readonly IReadOnlyList<Address> _addresses = new List<Address>();

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public IReadOnlyList<Address> Addresses => _addresses;

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