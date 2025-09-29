using ErrorOr;
using GeminiCustomer.Domain.Common.Models;
using GeminiCustomer.Domain.Customers.ValueObjects;

namespace GeminiCustomer.Domain.Customers.Entities;

public sealed class Address : Entity<AddressId>
{
    public CustomerId CustomerId { get; private set; }
    public string AddressLine1 { get; private set; }
    public string? AddressLine2 { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string PostCode { get; private set; }
    public string Country { get; private set; }
    public bool IsDefault { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    private Address(
        AddressId id,
        CustomerId customerId,
        string addressLine1,
        string? addressLine2,
        string city,
        string state,
        string postCode,
        string country,
        bool isDefault,
        DateTimeOffset createdAt,
        DateTimeOffset? updatedAt)
        : base(id)
    {
        CustomerId = customerId;
        AddressLine1 = addressLine1;
        AddressLine2 = addressLine2;
        City = city;
        State = state;
        PostCode = postCode;
        Country = country;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static ErrorOr<Address> Create(
        AddressId? id,
        CustomerId customerId,
        string addressLine1,
        string? addressLine2,
        string city,
        string state,
        string postCode,
        string country,
        bool isDefault,
        DateTimeOffset? createdAt,
        DateTimeOffset? updatedAt)
    {
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(addressLine1))
        {
            errors.Add(Error.Validation(
                code: "Address.AddressLine1.Empty",
                description: "Address line 1 must not be empty."));
        }
        else if (addressLine1.Length > 100)
        {
            errors.Add(Error.Validation(
                code: "Address.AddressLine1.TooLong",
                description: "Address line 1 must not exceed 100 characters."));
        }

        if (string.IsNullOrWhiteSpace(city))
        {
            errors.Add(Error.Validation(
                code: "Address.City.Empty",
                description: "City must not be empty."));
        }
        else if (city.Length > 50)
        {
            errors.Add(Error.Validation(
                code: "Address.City.TooLong",
                description: "City must not exceed 50 characters."));
        }

        if (string.IsNullOrWhiteSpace(state))
        {
            errors.Add(Error.Validation(
                code: "Address.State.Empty",
                description: "State must not be empty."));
        }
        else if (state.Length > 50)
        {
            errors.Add(Error.Validation(
                code: "Address.State.TooLong",
                description: "State must not exceed 50 characters."));
        }

        if (string.IsNullOrWhiteSpace(postCode))
        {
            errors.Add(Error.Validation(
                code: "Address.PostCode.Empty",
                description: "Post code must not be empty."));
        }
        else if (postCode.Length > 20)
        {
            errors.Add(Error.Validation(
                code: "Address.PostCode.TooLong",
                description: "Post code must not exceed 20 characters."));
        }

        if (string.IsNullOrWhiteSpace(country))
        {
            errors.Add(Error.Validation(
                code: "Address.Country.Empty",
                description: "Country must not be empty."));
        }
        else if (country.Length > 50)
        {
            errors.Add(Error.Validation(
                code: "Address.Country.TooLong",
                description: "Country must not exceed 50 characters."));
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        return new Address(
            id ?? AddressId.Create(),
            customerId,
            addressLine1,
            addressLine2,
            city,
            state,
            postCode,
            country,
            isDefault,
            createdAt ?? DateTimeOffset.UtcNow,
            updatedAt);
    }

    public void SetAsDefault()
    {
        IsDefault = true;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void UnsetDefault()
    {
        IsDefault = false;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}